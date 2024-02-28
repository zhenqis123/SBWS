// TODO: [Optional] Add copyright and license statement(s).

using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.Scripting;

namespace DefaultCompany.MRTK3.Subsystems
{
    [Preserve]
    [MRTKSubsystem(
        Name = "defaultcompany.mrtk3.subsystems",
        DisplayName = "DefaultCompany NewSubsystem",
        Author = "DefaultCompany",
        ProviderType = typeof(DefaultCompanyNewSubsystemProvider),
        SubsystemTypeOverride = typeof(DefaultCompanyNewSubsystem),
        ConfigType = typeof(NewSubsystemConfig))]
    public class DefaultCompanyNewSubsystem : NewSubsystem
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
            // Fetch subsystem metadata from the attribute.
            var cinfo = XRSubsystemHelpers.ConstructCinfo<DefaultCompanyNewSubsystem, NewSubsystemCinfo>();

            if (!DefaultCompanyNewSubsystem.Register(cinfo))
            {
                Debug.LogError($"Failed to register the {cinfo.Name} subsystem.");
            }
        }

        [Preserve]
        class DefaultCompanyNewSubsystemProvider : Provider
        {
            protected NewSubsystemConfig Config { get; }

            public DefaultCompanyNewSubsystemProvider() : base()
            {
                Config = XRSubsystemHelpers.GetConfiguration<NewSubsystemConfig, DefaultCompanyNewSubsystem>();
                
                // TODO: Apply the configuration to the provider.
            }

            #region INewSubsystem implementation

            // TODO: Add the provider implementation.

            #endregion NewSubsystem implementation
        }
    }
}
