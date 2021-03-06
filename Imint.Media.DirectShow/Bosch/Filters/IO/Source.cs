﻿// 
//  Source.cs
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
using Error = Kean.Core.Error;
using Kean.Core.Extension;
namespace Imint.Media.DirectShow.Bosch.Filters.IO
{
	public class Source :
		DirectShow.Binding.Filters.Guid
	{
		string file;
		public Source(string file, params DirectShow.Binding.Filters.Abstract[] next) :
			base(new System.Guid("86DFF86C-BB6C-4DBB-AE9A-914BC9B81233"), "VCS MPEG-4 File Source \"" + file + "\"", next) 
		{
			this.file = file;
			this.Output = 0;
		}
		public override DirectShowLib.IBaseFilter Create()
		{
			DirectShowLib.AMMediaType sourceMedia = new DirectShowLib.AMMediaType() { majorType = DirectShowLib.MediaType.Stream, subType = new Guid("555DC977-D6F1-4C2E-8D64-F58221A91234") };
			DirectShowLib.IBaseFilter result = base.Create();
			if(result.NotNull())
				DirectShow.Binding.Exception.GraphError.Check((result as DirectShowLib.IFileSourceFilter).Load(this.file, sourceMedia));
			return result;
		}
		public override bool Build(DirectShowLib.IPin source, DirectShow.Binding.IBuild build)
		{
			bool result = false;
			DirectShowLib.IBaseFilter filter = this.Create();
			if (build.Graph.AddFilter(filter, "VCS MPEG-4 File Source") == 0)
			{
				foreach (DirectShow.Binding.Filters.Abstract candidate in this.Next)
					if (result = candidate.Build(filter, build))
						break;
			}
			else
			{
				Error.Log.Append(Error.Level.Debug, "Unable to open Hauppauge Transport Reader.", "VCS MPEG-4 File Source was unable to open file \"" + this.file + "\".");
				DirectShow.Binding.Exception.GraphError.Check(build.Graph.RemoveFilter(filter));
			}
			return result;
		}
	}
}
