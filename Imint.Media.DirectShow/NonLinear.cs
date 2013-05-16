﻿// 
//  NonLinear.cs
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
using Kean.Core;
using Bitmap = Kean.Draw.Raster;
using Uri = Kean.Core.Uri;
using Kean.Core.Extension;

namespace Imint.Media.DirectShow
{    
    public abstract class NonLinear :
        Linear,
		Media.Player.INonLinear
    {
        protected NonLinear()
        {                
        }
        #region ILinear Members
		DateTime Media.Player.ILinear.Position
        {
            get { return this.Graph.Position; }
        }
        #endregion

        #region INonLinear Members
		bool Media.Player.INonLinear.IsNonLinear
		{
			get { return true; }
		}
		DateTime Media.Player.INonLinear.Start
        {
            get { return this.Graph.Start; }
        }

		DateTime Media.Player.INonLinear.End
        {
            get { return this.Graph.End; }
        }

		void Media.Player.INonLinear.Seek(DateTime position)
        {            
            this.Graph.Seek(position);
        }

        #endregion
	}
}