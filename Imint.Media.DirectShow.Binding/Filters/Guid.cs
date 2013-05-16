﻿// 
//  Guid.cs
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
    public class Guid :
        Creator
    {
		protected System.Guid Identifier { get; private set; }
        public Guid(System.Guid guid, string description) :
            base(description)
        {
            this.Identifier = guid;
        }
        public Guid(System.Guid guid, string description, params Filters.Abstract[] next) :
            base(description, next)
        {
            this.Identifier = guid;
        }
        public override DirectShowLib.IBaseFilter Create()
        {
            DirectShowLib.IBaseFilter result = null;
            try
            {
                result = Activator.CreateInstance(Type.GetTypeFromCLSID(this.Identifier)) as DirectShowLib.IBaseFilter;
            }
            catch(System.Exception e)
            {
                string message = "Filter \"" + this.Description + "\" could not find the guid \"" + this.Identifier.ToString() + "\".";
                new DirectShow.Binding.Exception.FilterNotFound(Error.Level.Debug, message, e.Message).Throw();
            }
            return result;
        }
    }
}