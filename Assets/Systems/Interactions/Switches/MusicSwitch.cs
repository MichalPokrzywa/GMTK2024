public class MusicSwitch : SwitchButton
{
    private void Start()
    {
        LoadMusicSettings();
    }

    public override void ChangeSwitchState()
    {
        base.ChangeSwitchState();

        AudioManager.MusicSettingsOn = !AudioManager.MusicSettingsOn;
    }

    public override void SetSwitchState(bool state)
    {
        base.SetSwitchState(state);

        AudioManager.MusicSettingsOn = state;
    }

    private void LoadMusicSettings()
    {
        SetSwitchState(AudioManager.MusicSettingsOn);
    }
}
