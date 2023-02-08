using System;
using FastGenerator.Utils;
using Siemens.Engineering;
using Siemens.Engineering.CustomIdentity;

namespace FastGenerator.Extensions
{
	/// <summary>
	/// Extensions methods used for the IEngineeringServiceProvider
	/// </summary>
	public static class ServiceProviderExtensions
	{
		public static string? GetIdentifier(this IEngineeringServiceProvider engineeringServiceProvider)
		{
			if (engineeringServiceProvider == null) 
				throw new ArgumentNullException(nameof(engineeringServiceProvider));
            
			var identityProvider = engineeringServiceProvider.GetService<CustomIdentityProvider>();

			return identityProvider?.Get(DeviceUtils.DeviceIdentifierKey);
		}

		public static void SetIdentifier(this IEngineeringServiceProvider engineeringServiceProvider, string identifier)
		{
			if (engineeringServiceProvider == null) 
				throw new ArgumentNullException(nameof(engineeringServiceProvider));
            
			var identityProvider = engineeringServiceProvider.GetService<CustomIdentityProvider>();

			identityProvider?.Set(DeviceUtils.DeviceIdentifierKey, identifier); 
		}
	}
}