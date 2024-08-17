public class SoundsSwitch : SwitchButton
{
    private void Start()
    {
        LoadSoundSettings();
    }

    public override void ChangeSwitchState()
    {
        base.ChangeSwitchState();

        AudioManager.SoundsSettingsOn = !AudioManager.SoundsSettingsOn;
    }

    public override void SetSwitchState(bool state)
    {
        base.SetSwitchState(state);

        AudioManager.SoundsSettingsOn = state;
    }

    private void LoadSoundSettings()
    {
        SetSwitchState(AudioManager.SoundsSettingsOn);
    }
}
