#!/bin/bash
apt update
apt install -y tesseract-ocr python3-pip portaudio19-dev python3-pyaudio
pip3 install --upgrade pip
pip3 install opencv-python pytesseract pyyaml typing_extensions six edge_impulse_linux paho-mqtt
# numpy 1.19.4 is the last working version, 1.19.5 does core dump on Nvidia Jetson platforms
pip3 install numpy==1.19.4 
