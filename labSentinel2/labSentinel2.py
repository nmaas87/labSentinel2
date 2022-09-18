# labSentinel 2
# Version 1.0.0
# Nico Maas, 2022-09-18
# https://github.com/nmaas87/labSentinel2

from distutils.log import debug
from typing_extensions import final
import cv2
from pytesseract import image_to_string
import yaml
from time import sleep
import sys
from edge_impulse_linux.image import ImageImpulseRunner
import paho.mqtt.client as mqtt

ocrElementsExist = False
coverUpElement = False
debugGUI = False
debugCLI = True
debugSave = False
videoMode = False
resize = True
resizeWidth = 320
resizeHeight = 320
outputSave = False
outputInterval = 1
frameCounter = 0
frameCounterTmp = 0
frameCounterTmp2 = 0
tesseractConfig = ('-l eng')
mqttAvailable = True
systemID = 1

def debugPrintCLI(text):
    if debugCLI:
        print(text)

def help():
    print("""
labSentinel 2
Nico Maas, 2022
https://github.com/nmaas87/labsentinel2

Starting without a parameter will run the sentinel mode

--demo
Shows what the model will see, no saving but GUI mode active

--capture
Useful to capture a running instance of the GUI to be monitored every x seconds. The GUI will be automatically cropped from the captured desktop, resized and saved to be used for later training of the edgeImpulse model
""")


def initRunner():
    global runner, labels
    if (modelFile==''):
        print("Error, no modelFile defined in config.yaml, inference not available")
        exit()
    else:
        runner = ImageImpulseRunner(modelFile)
        model_info = runner.init()
        #print(model_info)
        print('Loaded runner for "' + model_info['project']['owner'] + ' / ' + model_info['project']['name'] + '"')
        labels = model_info['model_parameters']['labels']

def initInput():
    if videoMode:
        try:
            global camera
            # open video device
            camera = cv2.VideoCapture(videoDevice)
            # set to MJPG mode (needs to be available with your device)
            camera.set(cv2.CAP_PROP_FOURCC, cv2.VideoWriter_fourcc('M', 'J', 'P', 'G'))
            # set video input width
            camera.set(cv2.CAP_PROP_FRAME_WIDTH, videoWidth)
            # set video input height
            camera.set(cv2.CAP_PROP_FRAME_HEIGHT, videoHeight)
            # set video input framerate to 1 frame per second
            camera.set(cv2.CAP_PROP_FPS, 1)
            # sleep 5 seconds for everything to settle
            sleep(5)
            if camera.isOpened():
                # capture first picture
                return_value, image = camera.read()
                # test if it worked
                if return_value:
                    print("Successfully opened camera and captured first picture")
                else:
                    print("Failed capturing picture, shutting down")
            else:
                print("Failed opening camera, shutting down")
        finally:
            print("Video System")
    else:
        try:
            image = cv2.imread(imageFile)
        finally:
            print("Image System")

def imageAcquisition():
    # Phase 00
    # load or acquire picture
    global frameCounter, image
    frameCounter+=1
    if videoMode:
        return_value, image = camera.read()
    else:
        image = cv2.imread(imageFile)
    if debugGUI:
        cv2.imshow('Phase 00',image)
        cv2.waitKey()
    if debugSave:
        cv2.imwrite('phase00_' + str(frameCounter) + '.png',image)

def findGUI():
    # Phase 01
    # find GUI on the Desktop and crop it
    # convert image to grayscale, and find edges
    global image, GUI
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    thresh = cv2.threshold(gray, 0, 255, cv2.THRESH_OTSU + cv2.THRESH_BINARY)[1]
    # find contour and sort by contour area
    cnts = cv2.findContours(thresh, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    cnts = cnts[0] if len(cnts) == 2 else cnts[1]
    cnts = sorted(cnts, key=cv2.contourArea, reverse=True)
    # find bounding box and extract GUI from Desktop
    for c in cnts:
        x,y,w,h = cv2.boundingRect(c)
        GUI = image[y:y+h, x:x+w]
        break
    if debugGUI:
        cv2.imshow('Phase 01',GUI)
        cv2.waitKey()
    if debugSave:
        cv2.imwrite('phase01_' + str(frameCounter) + '.png',GUI)

def cropPictureRunOCR():
    # Phase 02 
    # cropPicture
    # check if ocrElements are existing and has elements
    # if so, then crop the elements out of the GUI for OCR processing and run OCR,
    # otherwise skip crop and OCR steps
    if 'ocrElements' in config:
        # ocrElements dict exists
        if config['ocrElements']:
            ocrElementsExist = True
            debugPrintCLI('OCR Elements available, start cropping')
            # there are ocrElements in the dict, so we need to crop the areas to prepare the OCR process
            # crop elements and save in dict for later ocr
            for i in config['ocrElements']:
                crop_img = GUI[config['ocrElements'][i]['y']:config['ocrElements'][i]['y2'], config['ocrElements'][i]['x']:config['ocrElements'][i]['x2']]
                config['ocrElements'][i]['picture'] = crop_img
                if debugGUI:
                    cv2.imshow('Phase 02 ' + i,crop_img)
                    cv2.waitKey()
                if debugSave:
                    cv2.imwrite('phase02_' + i + '_' + str(frameCounter) + '.png',crop_img)
            #print(config['ocrElements'])

            # Phase 03
            # runOCR
            # run tesseract on pre-cropped ocrElements
            debugPrintCLI('OCR Elements available, start OCR')
            for i in config['ocrElements']:
                config['ocrElements'][i]['value'] = image_to_string(config['ocrElements'][i]['picture'],config=tesseractConfig).strip()
            # show elements
            if debugGUI:
                for i in config['ocrElements']:
                    cv2.imshow('Phase 03 ' + i + ":" + config['ocrElements'][i]['value'], config['ocrElements'][i]['picture'])
                    cv2.waitKey()
            # save the extracted text
            if debugSave:
                for i in config['ocrElements']:
                    with open('phase03_' + i + '_' + str(frameCounter) + '.txt', 'w') as f:
                        f.write(i + ":" + config['ocrElements'][i]['value'])                        
            # delete pre-cropped elements to save memory
            for i in config['ocrElements']:
                config['ocrElements'][i]['picture'] = None
            #print(config['ocrElements'])
    else:
        debugPrintCLI('No OCR Elements available, skipping cropping and OCR')

def coverUp():
    # Phase 04
    # coverUp
    # covering up GUI areas with pre-defined coverUpColor
    # examples are: sensitive areas, areas with changing values or timestamps that might irritate the inference process
    # if not configured differently, ocrElements areas will also be covered up
    # test if coverUpColor is configured, otherwise no coverUp process will take place
    if 'coverUpColor' in config:
        # exists, create coverUpColor tuple in format (blue, green, red)
        coverUpColor = (config['coverUpColor']['b'], config['coverUpColor']['g'], config['coverUpColor']['r'])

        # coverUp ocrElements
        if ocrElementsExist:
            # ocrElements dict exists, so we go through these elements
            for i in config['ocrElements']:
                # we look if the single element has either the key coverUp (then we grab it as coverUpElement)
                # or not, then we directly set coverUpElement = True
                if 'coverUp' in config['ocrElements'][i]:
                    coverUpElement = config['ocrElements'][i]['coverUp']
                else:
                    coverUpElement = True

                if coverUpElement:
                    # If coverUp is not configured or configured True, this OCR element will get covered up now
                    # if the key coverUp is configured as False, it will be spared by this process
                    cv2.rectangle(GUI, (config['ocrElements'][i]['x'], config['ocrElements'][i]['y']),  (config['ocrElements'][i]['x2'], config['ocrElements'][i]['y2']), coverUpColor, thickness=-1, lineType=cv2.LINE_8)
                    if debugGUI:
                        cv2.imshow('Phase 04-1 ' + i,GUI)
                        cv2.waitKey()
                    if debugSave:
                        cv2.imwrite('phase04-1_' + i + '_' + str(frameCounter) + '.png',GUI)
                    coverUpElement = False

        # coverUp coverUpElements
        if 'coverUpElements' in config:
            # coverUpElements dict exists, so we check that it is not empty
            if config['coverUpElements']:
                # there are coverUpElements in the dict, so we go through these elements and cover them up with color
                for i in config['coverUpElements']:
                    cv2.rectangle(GUI, (config['coverUpElements'][i]['x'], config['coverUpElements'][i]['y']),  (config['coverUpElements'][i]['x2'], config['coverUpElements'][i]['y2']), coverUpColor, thickness=-1, lineType=cv2.LINE_8)
                    if debugGUI:
                        cv2.imshow('Phase 04-2 ' + i,GUI)
                        cv2.waitKey()
                    if debugSave:
                        cv2.imwrite('phase04-2_' + i + '_' + str(frameCounter) + '.png',GUI)

def preprocessedReady():
    # Phase 05 
    # preprocessed picture ready
    # if OCR process was done, show values here
    if ocrElementsExist:
        for i in config['ocrElements']:
            print(i + ":" + config['ocrElements'][i]['value'])
    # show and write picture
    if debugGUI:
        cv2.imshow('Phase 05',GUI)
        cv2.waitKey()
    if debugSave:
        cv2.imwrite('phase05' + '_' + str(frameCounter) + '.png',GUI)

def resizePicture():
    # Phase 06
    # resize preprocessed picture
    # show picture size
    global frameCounterTmp, resized
    GUIShape = ''
    for item in GUI.shape:
        GUIShape = GUIShape + ' ' + str(item)
    debugPrintCLI("Original Dimensions: " + GUIShape)
    if (resize):
        # resize image
        resizeDim = (resizeWidth, resizeHeight)
        resized = cv2.resize(GUI, resizeDim, interpolation = cv2.INTER_AREA)
        resizedShape = ''
        for item in resized.shape:
            resizedShape = resizedShape + ' ' + str(item)
        debugPrintCLI('Resized Dimensions: ' + resizedShape)
        if debugGUI:
            cv2.imshow('Phase 06',resized)
            cv2.waitKey()
        if debugSave:
            cv2.imwrite('phase06' + '_' + str(frameCounter) + '.png',resized)
    # save pictures for training outside of debugSave functionality
    if (outputSave):
        if ((frameCounter - frameCounterTmp) >= outputInterval):
            cv2.imwrite('output' + str(frameCounter) + '.png',GUI)
            if (resize):
                cv2.imwrite('outputResize_' + str(frameCounter) + '.png',resized)
            frameCounterTmp = frameCounter

def runInference():
    global frameCounterTmp2
    global result_label, result_accuracy, result_time
    if (modelFile==''):
        print("Error, no modelFile defined in config.yaml, inference not available")
        exit()
    else:
        # imread returns images in BGR format, so we need to convert to RGB
        img = cv2.cvtColor(resized, cv2.COLOR_BGR2RGB)
        # get_features_from_image also takes a crop direction arguments in case you don't have square images
        features, cropped = runner.get_features_from_image(img)
        # classify the picture
        res = runner.classify(features)
        # sort the results for the highest value and get timing information
        result_label, result_accuracy = sorted(res['result']['classification'].items(), key=lambda kv: kv[1],reverse=True)[0]
        result_time = res['timing']['dsp'] + res['timing']['classification']
        # print results
        debugPrintCLI(result_label + ' (' + str(round(result_accuracy,2)) + '%) @ ' + str(result_time) + ' ms')
        # save pictures for training outside of debugSave functionality
        # the image will be resized and cropped, save a copy of the picture here
        # so you can see what's being passed into the classifier
        # it also includes the resulting label in the filename
        if (outputSave):
            if ((frameCounter - frameCounterTmp2) >= outputInterval):
                cv2.imwrite('outputInference_' + str(frameCounter) + '_' + result_label + '.png', cv2.cvtColor(cropped, cv2.COLOR_RGB2BGR))
                frameCounterTmp2 = frameCounter

def analyse():
    global systemID
    # system is in an error state, running OCR and report results
    if result_label == 'error':
        cropPictureRunOCR()
        ocrResults = ''
        for i in config['ocrElements']:
            ocrResults =  ocrResults + str(i) + ': ' + str(config['ocrElements'][i]['value']) + " / "
        message = systemID + " is in abnormal state: " + ocrResults
        print(message)
        reportMQTT(message)
    # system is in an ok state, report once in a while
    elif result_label == 'ok':
        message = systemID + " is in ok state"
        print(message)
        reportMQTT(message)
    # should never happen - so we report if it does
    else:
        cropPictureRunOCR()
        ocrResults = ''
        for i in config['ocrElements']:
            ocrResults =  ocrResults + str(i) + ': ' + str(config['ocrElements'][i]['value']) + " / "
        message = systemID + " is in " +  str(result_label) + " state: " + ocrResults
        print(message)
        reportMQTT(message)

def reportMQTT(message):
    if (mqttAvailable == False):
        print("Error, MQTT not defined in config.yaml, MQTT not available")
    else:
        client = mqtt.Client()
        client.username_pw_set(mqttUsername, mqttPassword)
        client.connect(mqttIP, int(mqttPort))
        client.publish(mqttTopic, str(message), qos=0, retain=False)
        client.loop()

def stop():
    runner.stop()


# Starting the main program here
## load yaml config file
try:
    with open("config.yaml", 'r') as configFile:
        config = yaml.safe_load(configFile)
        #print(config)

        # check if settings are available
        if 'config' in config:
            # check if debug is configured, then get debug settings
            if 'debug' in config['config']:
                debugGUI = config['config']['debug']['debugGUI']
                debugCLI = config['config']['debug']['debugCLI']
                debugSave = config['config']['debug']['debugSave']
                print("debugGUI: " + str(debugGUI))
                print("debugCLI: " + str(debugCLI))
                print("debugSave: " + str(debugSave))
            # check if system section is configured
            if 'system' in config['config']:
                # check if id exists
                if 'id' in config['config']['system']:
                    systemID = config['config']['system']['id']
                    print("System ID: " + str(systemID))
                else:
                    print("No system / id correctly configured, shutting down")
                    exit()
            else:
                print("Need system section in config.yaml, shutting down")
                exit()
            # check if input section is configured
            if 'input' in config['config']:
                # if videoDevice is configured, ignore static pictures
                if 'videoDevice' in config['config']['input']:
                    if 'videoWidth' in config['config']['input']:
                        if 'videoHeight' in config['config']['input']:
                            videoMode = True
                            videoDevice = config['config']['input']['videoDevice']
                            videoWidth = config['config']['input']['videoWidth']
                            videoHeight = config['config']['input']['videoHeight']
                            print("videoMode on")
                            print("videoDevice: " + videoDevice)
                        else:
                            print("No input / videoHeight correctly configured, shutting down")
                            exit()
                    else:
                        print("No input / videoWidth correctly configured, shutting down")
                # just as fallback or for training
                elif 'imageFile' in config['config']['input']:
                    videoMode = False
                    imageFile = config['config']['input']['imageFile']
                    print("videoMode off")
                    print("imageFile: " + imageFile)
                # if nothing is configured, we shutdown
                else:
                    print("No input correctly configured, shutting down")
                    exit()
            else:
                print("Need input section in config.yaml, shutting down")
                exit()
            # check if output section is configured
            if 'output' in config['config']:
                # resize section, loading values or setting to default
                if 'resize' in config['config']['output']:
                    resize = config['config']['output']['resize']
                    if (resize == True) and 'resizeWidth' in config['config']['output'] and 'resizeHeight' in config['config']['output']:
                        resizeWidth = config['config']['output']['resizeWidth']
                        resizeHeight = config['config']['output']['resizeHeight']
                    elif (resize == False):
                        resizeWidth = 0
                        resizeHeight = 0
                else:
                    print("No Output / resize not configured, falling back to resize = True with default 320x320")
                    resize = True
                    resizeWidth = 320
                    resizeHeight = 320
                print("resize: " + str(resize))
                print("resizeWidth: " + str(resizeWidth))
                print("resizeHeight: " + str(resizeHeight))
                # outputSave section, loading values or setting to default
                if 'outputSave' in config['config']['output']:
                    outputSave =  config['config']['output']['outputSave']
                else:
                    outputSave = False
                if 'outputInterval' in config['config']['output']:
                    outputInterval =  config['config']['output']['outputInterval']
                else:
                    outputInterval = 1
                print("outputSave: " + str(outputSave))
                print("outputInterval: " + str(outputInterval))
            else:
                print("Need output section in config.yaml, shutting down")
                exit()
            # check if ocr section is configured
            if 'ocr' in config['config']:
                #global tesseractConfig
                # loading values or setting to default
                if 'config' in config['config']['ocr']:
                    ocrConfig = config['config']['ocr']['config']
                    tesseractConfig = (ocrConfig)
                else:
                    print("No OCR / config configured, falling back to default -l eng")
                    tesseractConfig = ('-l eng')
            else:
                print("No OCR section in config.yaml, falling back to default -l eng")
                tesseractConfig = ('-l eng')
            # check if ai section is configured
            if 'ai' in config['config']:
                # resize section, loading values or setting to default
                if 'modelFile' in config['config']['ai']:
                    modelFile = config['config']['ai']['modelFile']
                else:
                    print("No AI / modelFile configured, only demoMode and captureMode will be available")
            else:
                print("Need ai section with configured modelFile in config.yaml, only demoMode and captureMode will be available")
                modelFile = ''
            # check if mqtt section is configured
            if 'mqtt' in config['config']:
                # mqtt section
                if 'ip' in config['config']['mqtt']:
                    mqttIP =  config['config']['mqtt']['ip']
                    if 'port' in config['config']['mqtt']:
                        mqttPort =  config['config']['mqtt']['port']
                        if 'username' in config['config']['mqtt']:
                            mqttUsername =  config['config']['mqtt']['username']
                            if 'password' in config['config']['mqtt']:
                                mqttPassword =  config['config']['mqtt']['password']
                                if 'topic' in config['config']['mqtt']:
                                    mqttTopic =  config['config']['mqtt']['topic']
                                else:
                                    print("No MQTT / topic configured, only demoMode and captureMode will be available")
                                    mqttAvailable = False
                            else:
                                print("No MQTT / password configured, only demoMode and captureMode will be available")
                                mqttAvailable = False
                        else:
                            print("No MQTT / username configured, only demoMode and captureMode will be available")
                            mqttAvailable = False
                    else:
                        print("No MQTT / port configured, only demoMode and captureMode will be available")
                        mqttAvailable = False
                else:
                    print("No MQTT / ip configured, only demoMode and captureMode will be available")
                    mqttAvailable = False
            else:
                print("Need mqtt section with configured ip, port, username, password in config.yaml, only demoMode and captureMode will be available")
                mqttAvailable = False
        else:
            print("Need config section in config.yaml, shutting down")
            exit()
except FileNotFoundError:
    print("No config.yaml found, shutting down")
    exit()
except yaml.YAMLError as error:
    print("There are errors in your config.yaml, shutting down")
    print(error)
    exit()

# start up program depending on needed mode
if len(sys.argv)==1:
    print("sentinelMode")
    initInput()
    initRunner()
    while True:
        imageAcquisition()
        findGUI()
        coverUp()
        resizePicture()
        runInference()
        analyse()
elif len(sys.argv)==2:
    if sys.argv[1]=="--demo":
        print("demoMode")
        debugGUI = True
        debugCLI = True
        debugSave = True
        outputSave = False
        initInput()
        while True:
            imageAcquisition()
            findGUI()
            coverUp()
            resizePicture()
    elif sys.argv[1]=="--capture":
        print("captureMode")
        debugGUI = False
        debugCLI = True
        debugSave = False
        outputSave = True
        outputInterval = 10
        initInput()
        while True:
            imageAcquisition()
            findGUI()
            coverUp()
            resizePicture()
    else:
        help()
else:
    help()

