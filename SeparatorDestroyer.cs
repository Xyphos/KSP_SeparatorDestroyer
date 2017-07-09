namespace SeparatorDestroyer
{
    public class SeparatorDestroyer : PartModule
    {
        [KSPField(isPersistant = true, guiName = "Destroyed when decoupled:", guiActive = true, guiActiveEditor = true)]
        [UI_Toggle(disabledText = "No", enabledText = "Yes", controlEnabled = false)]
        public bool Enabled = false;

        private ModuleDecouple _moduleDecouple;

        public override void OnAwake()
        {
            base.OnAwake();

            _moduleDecouple = part.Modules.GetModule<ModuleDecouple>();

            if (_moduleDecouple == null
                || _moduleDecouple.isOmniDecoupler)
                return;

            // normal decouplers shouldn't be considered and therefore nullified, with their GUI fields disabled.
            _moduleDecouple = null;
            Fields["Enabled"].guiActive = false;
            Fields["Enabled"].guiActiveEditor = false;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (Enabled
                && _moduleDecouple != null
                && _moduleDecouple.isDecoupled)
                part.explode();
        }
    }
}
