# NanoVGSharp Demo
### Summary
This demo is almost identical to the original NanoVG demo,
which have slightly modified to make it fit with C# and P/Invoke
layer.

Hotkeys:
+ Space: Switch Scale UP / Down the form widget
+ L: Framerate limiter switch
+ I: Framerate limit level up
+ K: Framerate limit level down
+ Esc: Exit

### Requirements
+ NanoVG compiled shared library binary with stb_image and font features, or at least any library that have exposed NanoVG API implementations.
+ ANGLE graphics translate layer library (You can simply get it if you are using MinGW)

### License
Demo source code are ported from: [memononen/nanovg](https://github.com/memononen/nanovg), also additional assets that included in this demo:
+ Roboto ([Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0))
+ Entypo ([CC BY-SA 4.0](https://creativecommons.org/licenses/by-sa/4.0/deed.en))
+ Noto Emoji ([SIL Open Font License 1.1](http://scripts.sil.org/cms/scripts/page.php?site_id=nrsi&id=OFL))
+ Images credit is in folder [assets/images.txt](./assets/images.txt)