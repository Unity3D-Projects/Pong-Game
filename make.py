#!/usr/bin/env python

import os, sys
sys.path.insert(0, os.path.join('build', 'pymake2'))
from pymake2 import *
from pymake2.template.csharp import csc

conf = { 'name': 'Pong.exe',

         'flags': ['/nologo',
                   #'/debug',
                   #'/define:DEBUG',
                   '/optimize',
                   '/target:winexe',
                   '/platform:x64'],

         'libdirs': csc.conf.libdirs + [ r'lib\SharpDX' ],

         'libs': [ 'PresentationCore.dll',
                   'System.IO.dll',
                   'System.Runtime.dll',
                   'WindowsBase.dll',

                   'SharpDX.D3DCompiler.dll',
                   'SharpDX.DXGI.dll',
                   'SharpDX.Direct3D11.dll',
                   'SharpDX.Mathematics.dll',
                   'SharpDX.XAudio2.dll',
                   'SharpDX.dll' ] }

@default_target(conf=csc.conf)
@depends_on('compile', 'content', 'libs')
def all(conf):
    pass

@target(conf=csc.conf)
def content(conf):
    copy('res', os.path.join(conf.bindir, 'Content'))

@target(conf=csc.conf)
def libs(conf):
    copy(r'lib\SharpDX', conf.bindir, '*.dll')

pymake2(conf)
