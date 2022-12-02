# Tauburn

Tauburn is the VRC MIDI STAR DRIVER. 

*  https://www.star-driver.net/


# Installation

## UPM with Git

Add the following to `Packages/manifest.json`.
```json
    "net.shivaduke28.tauburn": "https://github.com/shivaduke28/Tauburn.git?path=Packages/net.shivaduke28.tauburn",
```

To use the [AudioLink](https://github.com/llealloo/vrc-udon-audio-link) Bridge feature, add the following, too.
```json
    "net.shivaduke28.tauburn.audio-link": "https://github.com/shivaduke28/Tauburn.git?path=Packages/net.shivaduke28.tauburn.audio-link",
```

You can import AudioLink itself with UPM or VCC. Currently Tauburn supports AudioLink `v0.3.1`.

## VCC

- Download the zip file `net.shivaduke28.tauburn-*.*.*.zip` of the latest release  in the [release page](https://github.com/shivaduke28/Tauburn/releases).
- Unzip it and add it to VCC by **Settings > User Packages > Add**.
- To use AudioLink Bridge feature, you need to import AudioLink `v0.3.1` via VCC.