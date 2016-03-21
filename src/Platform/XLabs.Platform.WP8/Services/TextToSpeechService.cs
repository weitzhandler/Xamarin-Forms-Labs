// ***********************************************************************
// Assembly         : XLabs.Platform.WP8
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="TextToSpeechService.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Phone.Speech.Synthesis;

namespace XLabs.Platform.Services
{
	/// <summary>
	/// The text to speech service implements <see cref="ITextToSpeechService" /> for Windows Phone.
	/// </summary>
	public class TextToSpeechService : ITextToSpeechService
	{
		const string DEFAULT_LOCALE = "en-US";
		/// <summary>
		/// The _synth
		/// </summary>
		private readonly SpeechSynthesizer _synth;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextToSpeechService" /> class.
		/// </summary>
		public TextToSpeechService()
		{
			_synth = new SpeechSynthesizer();
		}

		/// <summary>
		/// The speak.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="language">The language.</param>
		public async void Speak (string text, string language = DEFAULT_LOCALE)
		{
			var voice = InstalledVoices.All.FirstOrDefault (c => c.Language == language) ?? InstalledVoices.Default;
			_synth.SetVoice (voice);
			await _synth.SpeakTextAsync (text);
		}

		/// <summary>
		/// Get installed languages.
		/// </summary>
		/// <returns>The installed language names.</returns>
		public IEnumerable<string> GetInstalledLanguages()
		{
			return InstalledVoices.All.Select(a => a.Language).Distinct();
		}
	}
}