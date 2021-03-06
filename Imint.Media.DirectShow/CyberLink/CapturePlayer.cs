﻿// 
//  CapturePlayer.cs
//  
//  Author:
//       Simon Mika <simon.mika@imint.se>
//  
//  Copyright (c) 2012-2013 Imint AB
// 
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//  * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//  the documentation and/or other materials provided with the distribution.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
using System;
using Kean.Core.Extension;
using Uri = Kean.Core.Uri;
using DirectShow = Imint.Media.DirectShow;

namespace Imint.Media.DirectShow.CyberLink
{
	public class CapturePlayer :
		DirectShow.Stream
	{
		protected override DirectShow.Binding.IGraph Open(Uri.Locator name)
		{
			DirectShow.Binding.IGraph result = null;
			if (name.Scheme == "directshow+capture" && name.Authority.NotNull() && name.Query["video"].IsEmpty())
			{
				DirectShow.Binding.Graph temporary = new DirectShow.Binding.Graph();
				bool built = temporary.Open(new DirectShow.Binding.Filters.Capture.All(name.Authority, new DirectShow.Binding.Filters.SampleGrabber.Yuyv(new DirectShow.Binding.Filters.NullRenderer()) { FuzzyMatch = this.Fuzzy }));
				if (built)
				{
					temporary.Play();
					System.Threading.Thread.Sleep(500);
				}
				temporary.Close();
				temporary = null;
				result = new DirectShow.Binding.Graph();
				built = result.Open(new DirectShow.Binding.Filters.Capture.All(name.Authority, new DirectShow.Binding.Filters.SampleGrabber.Yuyv(new DirectShow.Binding.Filters.NullRenderer()) { FuzzyMatch = this.Fuzzy }));
				if (built)
				{
					result.Play();
					System.Threading.Thread.Sleep(500);
					result.Close();
					built = result.Open(new DirectShow.Binding.Filters.Capture.All(name.Authority, new DirectShow.Binding.Filters.SampleGrabber.Yuyv(new DirectShow.Binding.Filters.NullRenderer()) { FuzzyMatch = this.Fuzzy }));
				}
				if (built)
					result.Play();
				else
				{
					result.Close();
					result = null;
				}
			}
			return result;
		}
	}
}
