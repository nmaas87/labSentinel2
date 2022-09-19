# labSentinel2

Project by Nico Maas, 2022

[Quick Introduction / Blog Post](https://www.nico-maas.de/?p=2498)

[Youtube Demo and Setup Guide](https://youtu.be/KEN_HT20exs)

Sponsored and powered by Advantech AIR-020X and Edge Impulse as part of the Advantech Edge AI Challenge 2022:

<p align="center">
  <img width="600" src="images\advantech.jpg">
</p>

... now also part of Spark and Nvidias [#SummerOfJetson](https://twitter.com/nmaas87/status/1571584031988252680)

## Setting the scene

Working in the Aerospace Industry - with additional roots in Material Sciences and Life Sciences comes with the perk of having to work in laboratories around quite expensive - and old - machinery. Most of this expensive lab equipment has been bought back in the '90s and is controlled by x86 computers running either Windows NT, Windows 95 or - in the most advanced cases - Windows XP. For most of these machines, no software updates are available (or support for modern OS) - and they have another problem: They cannot integrate into modern monitoring solutions - or any monitoring at all. These tools were made with human operators or technicians in mind who would monitor the equipment 24/7 - waiting for some virtual red led of big warning message to appear. Running a lab with dozens of such machines, which shall operate 24/7 with as little downtime as possible can be an expensive and mundane task, which this project tries to solve.

## The proposed solution

Advantech AIR-020 series and Edge Impulse are the ideal and cost-effective solution for this problem: Attaching an HDMI Grabber including necessary adapters (e.g. for VGA or DVI outputs of the lab equipment) allows to train a classification model which can recognize "good" and "bad" states and then alert supervisor - or even turn off power supply to the equipment if something goes really wrong. The best use of such a solution is in combination with a technician to lessen the workload and allow for more effective usage of human resources.

<p align="center">
  <img width="600" src="images\overview.jpg">
</p>



## Proof of concept

Youtube Demo and Setup Guide: [https://youtu.be/KEN_HT20exs](https://youtu.be/KEN_HT20exs)

### Needed hardware / software

- [Advantech AIR-020 series board](https://www.advantech.eu/products/65f20c25-f6ef-4ab5-be3c-b7dfa7a833b3/air-020/mod_fcf216c8-3495-4809-b815-61dc008d53a4) with power supply and Jetson Jetpack 4.6.1 software (or NVIDIA Jetson Nano 2 GB/4 GB with Jetson Jetpack 4.6.1, not recommended in an Industrial / Production environment)
- [Free Edge Impulse Account](https://studio.edgeimpulse.com/signup)
- Videograbber
    - either one of the cheap "HDMI Video Capture" USB cards from china
    - or one of the better devices with pass-through for the monitor signal (e.g. Elgato and similar)

### LabDeviceControlSoftware / Demo GUI

No idea without a proof of concept, for this use-case I designed a small program called "LabDeviceControlSoftware". It was written in VB.Net (Visual Studio 2019) to have a more faithful recreation of the old softwares look and feel. The source code is included in the LabDeviceControlSoftware folder, as well as an already compiled version ( LabDeviceControlSoftware\LabDeviceControlSoftware\bin\Release )

<p align="center">
  <img width="700" src="images\demosoftware.png">
</p>

In this software, we have actually 7 different "states" we can picture our imaginary power supply to be in:

- ok: all is on "OK", the mode we want
- Errors:
  - errorFive: the 5 V rail shows an error
  - errorOverCurrent: overcurrent (output) error
  - errorOverTemperature: overtemperature error
  - errorOverVoltage: overvoltage (input) error
  - errorThree: the 3.3 V rail shows an error
  - errorTwelve: the 12 V rail shows an error

To allow for easier training, the software includes multiple key bindings to switch the modes:

- You can click the textlabels to switch "OK" to "Error" and back
- The number keys 1-6 are also bound to the same functionality
- The "r" key moves the window on your desktop to a new position, this is very useful for training

### Installation and Demo

On your Jetson device, clone the repo and start the installer with sudo rights or as root. It will install all needed dependencies and downgrade numpy to fix an error currently existing on JetPack. As always I recommend checking the code before running something with root / sudo rights.

````
git clone https://github.com/nmaas87/labSentinel2.git
cd labSentinel2/labSentinel2
chmod +x install.sh
sudo ./install.sh
````

After the installation of software and hardware, you will need to setup the labSentinel to use your grabber card, capture training data for both the ````ok```` and ````error```` states, train a model with Edge Impulse and include the model. This is shown in detail in the attached Youtube Demo above. If you just want to run the included demo you can use the pretrained model and setup like below. We assume you got your Jetson ready and setup, connected to the internet, you added a Videograbber and attached another PC running the demo software of the GUI - with its screen shared to the Videograbber.

- Within the config.yaml configure labSentinel to use the first video capture card on your pc instead of a example file by changing
````
  input:
    #videoDevice: '/dev/video0'
    #videoWidth: 1920
    #videoHeight: 1080
    imageFile: 'input.png'
````
to
````
  input:
    videoDevice: '/dev/video0'
    videoWidth: 1920
    videoHeight: 1080
    #imageFile: 'input.png'
````
- make the included model file executable (````chmod +x labsentinel2-linux-aarch64-v1.eim````)
- run the software in sentinelMode (````python3 labSentinel2.py````)
- from this moment on, it will send MQTT messages to following MQTT server and topic:
````
  mqtt:
    ip: 'public.cloud.shiftr.io'
    port: '1883'
    username: 'public'
    password: 'public'
    topic: 'labSentinel2/unit4711'
````
To learn more about labSentinel and how it works, read on :).


### Deployment and use in real world

In a real world scenario, one would deploy labSentinel in an Docker container to make it easier to version and keep up running. Maybe we will see [balena](https://www.balena.io) support for the AIR-020 series one day, this would be the ideal solution for deploying in the field.



## labSentinel and its modes

The labSentinel comes pre-configured with a demo project. But it is important to understand the processing pipeline and configuration items first, so you can tailor it to your needs.

labSentinel can run in three modes:
- sentinelMode
- demoMode
- captureMode

Depending on the mode, different modules and settings are active, which depend on the main configuration file (config.yaml) for their parameters.

### sentinelMode

This will be the main mode to run for supervision of a system (````python3 labSentinel2.py````). It will start by ````initInput````, which will initialize the video system. Followed by ````initRunner```` which will load the inference model to be used. After this has been successful, the following while loop will do the following: Getting a new image (````imageAcquisition````), extracting the GUI from the grabbed picture (````findGUI````), painting over sensitive areas like password or not needed information for the inference (````coverUp````), resizing the picture so that it becomes useful for the model (````resizePicture````), finally running the model on the extracted and preprocessed picture (````runInference````) and the analysis of the found information (````analyse````). Within the ````analyse```` step, either the result of the inference was ````ok```` - which will then be sent via MQTT to a monitoring server, or it was ````error````. If thats a case, a module called ````cropPictureRunOCR```` can be run. It will extract information from the original picture via X,Y coordinates and run OCR on it. This is so that e.g. voltages or other important informations can be extracted. The found information will be sent as a warning via MQTT to a monitoring server.

### demoMode

This mode can be started with the ````--demo```` tag (````python3 labSentinel2.py --demo````). It will run through all modules except ````initRunner````, ````runInference```` and ````analyse````. It is meant to visualize how labSentinel breaks down the information and can be used to tweak settings. It will also save the pictures of the different steps and show the executions in a GUI.

### captureMode

The last mode can be started with the ````--capture```` tag (````python3 labSentinel2.py --capture````). It will invoke the same modules as ````--demo````, but not show the steps in a GUI and only save the resulting picture in the resized and "bigger" version. This mode is meant to capture live data to be used for the development of the model later used for the inference purpose.


## Configuration & Processing Pipeline

To see all the configuration options, open up the config.yaml and let me explain how this works:

````
config:
  debug:
    debugGUI: False
    debugCLI: True
    debugSave: False
  system:
    id: 'labPSU#4711'
  input:
    #videoDevice: '/dev/video0'
    #videoWidth: 1920
    #videoHeight: 1080
    imageFile: 'input.png'
  output:
    resize: True
    resizeWidth: 320
    resizeHeight: 320
    outputSave: False
    outputInterval: 10
  ocr:
    config: '-l eng'
  ai:
    modelFile: './labsentinel2-linux-aarch64-v1.eim'
  mqtt:
    ip: 'public.cloud.shiftr.io'
    port: '1883'
    username: 'public'
    password: 'public'
    topic: 'labSentinel2/unit4711'

ocrElements:
  3v3:
    x: 180
    y: 92
    x2: 380
    y2: 140
  5v:
    x: 180
    y: 145
    x2: 380
    y2: 192
  12v:
    x: 180
    y: 198
    x2: 380
    y2: 246
coverUpColor:
  r: 240
  g: 240
  b: 240
coverUpElements:
  ip:
    x: 180
    y: 409
    x2: 777
    y2: 461
  password:
    x: 180
    y: 460
    x2: 777
    y2: 514
````

### Debug Switches:

Within the config tree, the first item is the debug area, including debugGUI and debugCLI.

Configuration:

````
config:
  debug:
    debugGUI: False
    debugCLI: True
    debugSave: False
````

````debugCLI```` will output additional information on the command line interface, while ````debugGUI```` will show the graphical representation of each step on screen, waiting for a spacebar press to go over to the next step. Running the labSentinel with ````debugGUI: True```` in a production environment is not possible, but it is great for debugging.  ````debugSave```` will save each picture onto the harddrive for debugging purposes. Should not be run too long, otherwise the harddrive will fill up fast. It is wise to use ````debugGUI```` and ````debugSave```` in conjunction with each other, just stepping to one or two images, shutdown the program, analyze and adapt the code. Using the modes like sentinelMode, demoMode or captureMode will overwrite these switches, depending on the needed settings.

### Module: imageAcquisition

In this phase, either a static image will be loaded or a videoDevice will be opened to start grabbing pictures.
Either ````videoDevice```` with its ````videoWidth```` and ````videoHeight```` has to be set, or the ````ìmageFile````. The system will then initalize either the video hardware with the correct input width/height for the screen signal, or load a picture from a file to start the preprocessing.

Configuration:

````
  input:
    #videoDevice: '/dev/video0'
    #videoWidth: 1920
    #videoHeight: 1080
    imageFile: 'input.png'
````

The input:

<p align="center">
  <img width="700" src="images\phase00_1.png">
</p>

### Module: findGUI

As the captured image is way to noisy and too big for inference, this phase will run OpenCV to isolate the GUI from the Desktop and crop it into a smaller picture. It is important that both Desktop and Window/GUI can be visually distinguished easily from each other, e.g. by using contrasting colors. In this example we choose a darker background against the light coloured GUI. Using a black background would be even better. In contrast, a darker GUI would be better suited to be shown on a white Desktop to help the algorithm find the edges and extract the GUI. It is also given that the GUI either a) cannot be resized or will not be resized during operations as no human should be using the system. It could however be moved.

The extracted GUI:

<p align="center">
  <img width="700" src="images\phase01_1.png">
</p>

### Module: coverUp

In Phase 02, a pre-defined color will be loaded (values of red, green and blue can be configured as ````coverUpColor````) which will then be applied to ````coverUpElements````. The example shows 2 areas defined by their internally used name (````ip```` and ````password````), coordinates (````x````,````y```` and ````x2````,````y2````). Defined areas will be covered with the ````coverUpColor```` to directly erase sensitive information (like password, IP addresses) or too specific and rapidly changing information which could impact the inference process. If no ````coverUpElements```` are defined, Phase 02 is ignored.

Configuration:

````
coverUpColor:
  r: 240
  g: 240
  b: 240
coverUpElements:
  ip:
    x: 180
    y: 409
    x2: 777
    y2: 461
  password:
    x: 180
    y: 460
    x2: 777
    y2: 514
````

Cover up the IP field:

<p align="center">
  <img width="700" src="images\phase04-2_ip_1.png">
</p>

Cover up the Password field:

<p align="center">
  <img width="700" src="images\phase04-2_password_1.png">
</p>



### Module: cropPictureRunOCR

As the GUI probably also contains different areas of interest showing text which can be useful for understanding and crosschecking the current status of the instrument, these areas can be set with ````ocrElements```` in the ````config.yaml```` to be extracted and then run through tesseractOCR for text extraction.

Configuration:

````
config:
  [...]
  ocr:
    config: '-l eng'
  [...]
ocrElements:
  3v3:
    x: 180
    y: 92
    x2: 380
    y2: 140
  5v:
    x: 180
    y: 145
    x2: 380
    y2: 192
  12v:
    x: 180
    y: 198
    x2: 380
    y2: 246
````

This example shows 3 areas defined by their internally used name (````3v3````, ````5v````, ````12v````), coordinates (````x````,````y```` and ````x2````,````y2````) and as well as a ````coverUp```` boolean value.

If you do not define the ocrElements section at all, this will be skipped. Otherwise, you need to set it up properly: You have to provide a name as well as the coordinates for the rectangular crop to be made within the context of the in findGUI already isolated GUI.

Showcasing the coordinate system:

<p align="center">
  <img width="700" src="images\phase02_1_3v3_example.png">
</p>

After extracting the picture of the area of interest, it will be stored and then run through tesseract OCR.

The extracted 3v3 field:

<p align="center">
  <img width="700" src="images\phase02_3v3_1.png">
</p>

OCR Values:

````
3v3:1,53
````

Also there is a config section for the tesseract module. You can find more about this within the manual of [tesseract](https://digi.bib.uni-mannheim.de/tesseract/manuals/tesseract.1.html).

### Module: resizePicture

Within the config area of the ````config.yaml```` is an additional ````output```` section. This does allow to ````resize```` the picture to a certain width and height, represented by ````resizeWidth```` and ````resizeHeight````. If ````resize```` is not configured at all, it will still be enabled to 320x320, as it will probably be needed later for the inference process. Only setting it explicitly to ````resize: False```` will disable it. An additional feature is ````outputSave````. It will export both the original and the resized (if resize is enabled) picture every x frames (````outputInterval````) and save them to the harddrive. This is especially useful to gather pictures in a live system for training of the inference system. ````outputSave```` works independently from the debug options, but will need a lot of space if you forget to disable it.

Configuration:

````
  output:
    resize: True
    resizeWidth: 320
    resizeHeight: 320
    outputSave: False
    outputInterval: 10
````

Resized image, ready for inference:

<p align="center">
  <img width="320" src="images\phase06_1.png">
</p>

### Module: initRunner / runInference

The edgeImpulse runner needs a model generated for the specific usecase / inference it shall provide. The downloaded model file for linux/aarch64 has to be provided within the same folder and made executable (````chmod +x file.eim````) so that it can be used.

Configuration:

````
config:
  [...]
  ai:
    modelFile: './labsentineltest-linux-aarch64-v1.eim'
  [...]

````

### Module: analyse

The ````analyse```` module will look into the results of ````runInference````. If it detects the result ````error````, it will run ````cropPictureRunOCR```` to get more information and post an error message to a preconfigured MQTT server by using the system id as a identifiable name in the message. Also, if everything was ````ok````, it will also send a message, so that one knows the system is still alive and performing its duties.

Configuration:

````
config:
  [...]
  system:
    id: 'labPSU#4711'  
  [...]
  mqtt:
    ip: 'public.cloud.shiftr.io'
    port: '1883'
    username: 'public'
    password: 'public'
    topic: 'labSentinel2/unit4711'
  [...]

````

## Conclusion

As seen, labSentinel is a cost effective solution to monitor legacy equipment and make sure its operations are safeguarded, even with no reporting functionality available in the legacy system. A single Jetson system is powerful enough to monitor multiple legacy systems and if it were to be used for critical operations, it would even be able to cut off power to the legacy equipment by using ethernet connected wallplugs. Thanks again to Advantech and Edge Impulse for the support and making the second version of labSentinel a reality.

## Disclaimer / Transparency

As part of the Advantech Edge AI Challenge 2022, Advantech provided me with one AIR-020X-S9A1 at no additional charge to make this project possible. Special thanks to Gary Lin (Advantech) as well as Louis Moreau and David Tischler (Edge Impulse) for their support :)!
