#!/usr/bin/env python
import os, sys
sys.path.insert(0, os.path.join('build', 'pymake'))
from pymake import *

import csc

@target
@depends_on('compile', 'content', 'libs')
def all(conf):
    pass

@target
def content(conf):
    copy('res', os.path.join(conf.bindir, 'Content'))

@target
def libs(conf):
    copy(r'lib\SharpDX', conf.bindir, '*.dll')

pymake(csc.defaultConf(), {
    'name': 'Pong.exe',

    'flags': ['/target:winexe',
              #'/debug',
              #'/define:DEBUG',
              '/o',
              '/platform:x64'],

    'libs': ['PresentationCore.dll',
             'System.IO.dll',
             'System.Runtime.dll',
             'WindowsBase.dll',

             'SharpDX.D3DCompiler.dll',
             'SharpDX.DXGI.dll',
             'SharpDX.Direct3D11.dll',
             'SharpDX.Mathematics.dll',
             'SharpDX.XAudio2.dll',
             'SharpDX.dll'],

    'libdirs': [r'lib\SharpDX'],
})
