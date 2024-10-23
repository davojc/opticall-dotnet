# opticall-dotnet


## Install on Linux

### Dependencies

```
sudo
```

```
curl
```

Download and run the bash script found at: https://github.com/davojc/opticall-dotnet/releases/latest/download/deploy.sh

### Verify installed

```
systemtcl status opticall.service
```



## Configuring the endpoint

Each endpoint is configured with a Target Id and a Group Id. This will allow sending messages to specific device (Target) or a group of devices (Group) if broadcast is used.

When opticall is installed, it copies in a default configuration file:

```json
{
    "Target": "Default",
    "Group": "Group",
    "Port": 8765
}
```

## Sending OSC commands

Color Commands

|Path|Description|Args|
|-|-|-|
|/$target/led/on|Turns the LED on with the specified color|$leds $r $g $b|
|/$target/led/off|Turns the LED off|$leds|
|/$target/led/strobe|Will set the LED to strobes with a given color at a given speed for a given number of times. Once the strobe is complete it will return to the previous state.|$leds $r $g $b $speed $repeat
|/$target/led/pattern|Activates one of the built in patterns|$leds $pattern $repeat| 
|/$target/led/fade|The LED will fade to the given color over the given time.|$leds $r $g $b %time|
|/$target/led/wave|Creates a wave along the LED of a given color|

See the following for specific details for each command

### led/on

|Name|Position|Values|
|-|-|-|
|Which LED to turn on|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|

### led/off

|Name|Position|Values|
|-|-|-|
|Which LED to effect|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|

### led/strobe

|Name|Position|Values|
|-|-|-|
|Which LED to strobe|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|
|Speed|5|0-255|
|Repeat|6|0-255|

### led/pattern

|Name|Position|Values|
|-|-|-|
|Which pattern to apply|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Pattern|2|TrafficLight - 1, Random1 -2, Random2 - 3, Random3 - 4, Police = 5, Random4 = 6, Random5 = 7, Random6 = 8 |
|Time|3|0-255|

### led/fade

|Name|Position|Values|
|-|-|-|
|Which LED to fade|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|
|Time|5|0-255|

### led/wave

|Name|Position|Values|
|-|-|-|
|Which LED to wave|1|All - 255, Front - 65, Back - 66, One - 1, Two - 2, Three - 3, Four - 4, Five - 5, Size - 6|
|Red|2|0-255|
|Green|3|0-255|
|Blue|4|0-255|
|Repeat|5|0-255|
|Speed|6|0-255|
