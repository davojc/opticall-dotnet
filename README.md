# opticall-dotnet

## Service Config

The optical service has only 3 config settings:

|Settings|Description|Default|
|-|-|-|
|target|This is the id of the target. Using this means that you can broadcast messages but only the target signal will react.|target|
|group|This is a group which can be used so that when you broadcast only members of the group will respond|group|
|port|This is the port to listen on.|8765|

To see the different options for configuring these settings.

### Config file

When installed, the service will be set up with a config file 'settings.yml' in the same directory as the 

```
settings.yml
```

This file contains the above 3 settings so can be modified to the desired settings.

### On deployment

When installing you will have the option to specify the target and/or group for the service. See installing for details on the arguments you can pass to the deployment scripts.

### Via OSC command

The config setting 'target' and 'group' can be modified by sending an OSC command. Be aware that if you make changes this way that it will stop responding to the old 'target' or 'group'. This change is also persisted, it will write the change to the configuration file.

|Path|Args|
|-|-|
|/$target/config/target|New target|
|/$target/config/group|New group|

## Install on Linux

### Dependencies

- sudo
- curl

### Steps

- Download the deployment bash script
```bash
wget -O deploy.sh https://github.com/davojc/opticall-dotnet/releases/latest/download/deploy.sh
```
- Run the bash script (see below for args)

|Arg|
|-|
|-t customTarget|
|-g customGroup|

### Verify install

The service binaries and config will be installed in the location: '/usr/local/bin/opticall'.

```
systemtcl status opticall.service
```

## Install on Windows

### Steps

- Open a powershell console as administrator.

- Set the execution policy
```
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope Process
```

- Download the deployment script using the following:
```
Invoke-WebRequest https://github.com/davojc/opticall-dotnet/releases/latest/download/deploy.ps1 -OutFile deploy.ps1
```
- Run the powershell script (see below for args)

|Arg|
|-|
|-t customTarget|
|-g customGroup|

### Verify Install

The service binaries and config will be installed in the location: "C:\Program Files\Opticall"

You should find a Windows Service in the Services console called 'Opticall'.



## Sending OSC commands

### Color Commands

|Path|Description|Args|
|-|-|-|
|/$target/led/on|Turns the LED on with the specified color|$leds $r $g $b|
|/$target/led/off|Turns the LED off|$leds|
|/$target/led/strobe|Will set the LED to strobes with a given color<br /> at a given speed for a given number of times. <br/>Once the strobe is complete it will return to the previous state.|$leds $r $g $b $speed $repeat
|/$target/led/pattern|Activates one of the built in patterns|$leds $pattern $repeat| 
|/$target/led/fade|The LED will fade to the given color over the given time.|$leds $r $g $b %time|
|/$target/led/wave|Creates a wave along the LED of a given color|$type $r $g $b $speed $repeat

See the following for specific details for each command

#### led/on

|Name|Position|Values|
|-|-|-|
|Which LED to turn on|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|

#### led/off

|Name|Position|Values|
|-|-|-|
|Which LED to effect|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|

#### led/strobe

|Name|Position|Values|
|-|-|-|
|Which LED to strobe|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|
|Speed|5|0-255|
|Repeat|6|0-255|

#### led/pattern

|Name|Position|Values|
|-|-|-|
|Pattern|1|TrafficLight - 1, Random1 -2, Random2 - 3, Random3 - 4, Police = 5, Random4 = 6, Random5 = 7, Random6 = 8 |
|Repeat|2|0-255|

#### led/fade

|Name|Position|Values|
|-|-|-|
|Which LED to fade|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|
|Time|5|0-255|

#### led/wave

|Name|Position|Values|
|-|-|-|
|Wave Function|1|Short - 1, Long - 2, Overlapping Short - 3, Overlapping Long - 4, Other - 5|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|
|Speed|5|0-255|
|Repeat|6|0-255|
