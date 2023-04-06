using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSettings : StaticInstance<StoreSettings>
{
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;
    public float NarratorVolume;

    public int Window;
    public int Graphics;
    public int Resolution;
}
