﻿// 
//  Creator.cs
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
using Log = Kean.Platform.Log;
using Error = Kean.Core.Error;

namespace Imint.Media.DirectShow.Binding.Filters
{
    public abstract class Creator :
        Abstract
    {
		public TimeSpan WaitForOutput { get; set; }
		public Creator(string description, params Filters.Abstract[][] followers) :
            base(description, followers)
        { }
        public abstract DirectShowLib.IBaseFilter Create();
        protected void CreateSource(DirectShowLib.DsGuid pinCategory, DirectShowLib.IBaseFilter source, DirectShowLib.IBaseFilter destination, IBuild build)
        {
            DirectShowLib.ICaptureGraphBuilder2 captureGraph = new DirectShowLib.CaptureGraphBuilder2() as DirectShowLib.ICaptureGraphBuilder2;
            Exception.GraphError.Check(captureGraph.SetFiltergraph(build.Graph));
            Exception.GraphError.Check(captureGraph.RenderStream(pinCategory, DirectShowLib.MediaType.Video, source, null, destination));
        }
		protected virtual bool PreConfiguration(IBuild build) { return true; }
		protected virtual bool PostConfiguration(IBuild build) { return true; }
        public override bool Build(DirectShowLib.IPin source, IBuild build)
        {
            bool result = false;
            DirectShowLib.IBaseFilter filter = this.Create();
            if (filter.NotNull() && build.Graph.AddFilter(filter, this.Description) == 0 && this.PreConfiguration(build))
            {
                result = true;
                DirectShowLib.PinInfo pinInformation;
                Exception.GraphError.Check(source.QueryPinInfo(out pinInformation));
                DirectShowLib.FilterInfo filterInformation;
                Exception.GraphError.Check(pinInformation.filter.QueryFilterInfo(out filterInformation));
                switch (filterInformation.achName)
                {
                    case "Capture":
                        this.CreateSource(DirectShowLib.PinCategory.Capture, pinInformation.filter, filter, build);
                        break;
                    case "Source":
                        this.CreateSource(null, pinInformation.filter, filter, build);
                        break;
                    default:
                        if (!(result = (this.FuzzyMatch ?
                            0 <= build.Graph.Connect(source, DirectShowLib.DsFindPin.ByDirection(filter, DirectShowLib.PinDirection.Input, 0)) :
                            0 == build.Graph.ConnectDirect(source, DirectShowLib.DsFindPin.ByDirection(filter, DirectShowLib.PinDirection.Input, 0), new DirectShowLib.AMMediaType()))))
                        {
							Log.Cache.Log(Error.Level.Debug, "Unable to connect.", "DirectShow was unable to connect \"" + filterInformation.achName + "\" with \"" + this.Description + "\".");
                            Exception.GraphError.Check(source.Disconnect());
                            Exception.GraphError.Check(build.Graph.RemoveFilter(filter));
                        }
                        break;
                }
				if (result &= this.PostConfiguration(build))
				{
					if (this.WaitForOutput.Ticks > 0)
						System.Threading.Thread.Sleep(this.WaitForOutput);
					for (int i = 0; i < this.Followers.Length; i++)
						foreach (Filters.Abstract candidate in this.Followers[i])
							if (result &= candidate.Build(filter, build))
								break;
				}
            }
			return result;
        }
    }
}
