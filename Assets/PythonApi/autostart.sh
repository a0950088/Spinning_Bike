#!/bin/bash
source ~/spinning_bike/bin/activate
cd /home/pi/spinning_bike/project/bluepy/
sudo /home/pi/spinning_bike/bin/python main.py -i "192.168.100.145" -p 30000
