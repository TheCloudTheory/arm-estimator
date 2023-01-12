namespace ACE.WhatIf
{
    internal class TerraformTemplateParser
    {
        private readonly string template;

        public TerraformTemplateParser(string template)
        {
            this.template = template;
        }

        public WhatIfResponse? GetConfigurationAsWhatIfData()
        {
            return null;
        }
    }
}
