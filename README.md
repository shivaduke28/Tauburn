# Tauburn

Tauburn is the VRC MIDI STAR DRIVER. 

*  https://www.star-driver.net/


# Installation

## UPM with Git

Add the following to `Packages/manifest.json`.
```json
    "net.shivaduke28.tauburn": "https://github.com/shivaduke28/Tauburn.git?path=Packages/net.shivaduke28.tauburn#0.0.2",
```

To use [AudioLink](https://github.com/llealloo/vrc-udon-audio-link) Brdige feature, add the following:
```json
    "com.llealloo.audiolink": "https://github.com/llealloo/vrc-udon-audio-link.git?path=Packages/com.llealloo.audiolink#d9de6e29181bd941efae31cdadda00582b26eaae",
    "net.shivaduke28.tauburn.audio-link": "https://github.com/shivaduke28/Tauburn.git?path=Packages/net.shivaduke28.tauburn.audio-link#0.0.2",
```

Note that in the current version (`v0.0.2`) Tauburn's AudioLink Bridge depends on the revision [d9de6e2](https://github.com/llealloo/vrc-udon-audio-link/tree/d9de6e29181bd941efae31cdadda00582b26eaae) of AudioLink that is not released yet.


## VCC

- Download the zip file `net.shivaduke28.tauburn-*.*.*.zip` of the lastest release  in the [release page](https://github.com/shivaduke28/Tauburn/releases).
- Unzip it and add it to VCC by **Settings > User Packages > Add**.

To use AudioLink Bridge feature, you need to import AudioLink `v0.3.X` via VCC.

- Download a zip file of AudioLink from [here](https://github.com/llealloo/vrc-udon-audio-link/archive/d9de6e29181bd941efae31cdadda00582b26eaae.zip).
- Unzip it and add `vrc-udon-audio-link-dev/vrc-udon-audio-link-dev/Packages/com.llealloo.audiolink` directory to VCC.
- Then download `net.shivaduke28.tauburn.audio-link-*.*.*.zip` and add it to VCC as above.
