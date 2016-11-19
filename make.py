#!/usr/bin/env python
import os, sys
sys.path.insert(0, os.path.join('build', 'pymake'))
from pymake import *

NAME = 'Pong.exe'

FLAGS = ['/nologo',
         '/o',
         '/platform:x64',
         '/target:winexe']

LIBS = ['PresentationCore.dll',
        'System.IO.dll',
        'System.Runtime.dll',
        'WindowsBase.dll',

        'SharpDX.D3DCompiler.dll',
        'SharpDX.DXGI.dll',
        'SharpDX.Direct3D11.dll',
        'SharpDX.Mathematics.dll',
        'SharpDX.XAudio2.dll',
        'SharpDX.dll']

LIBDIRS = [r'C:\Windows\Microsoft.NET\Framework64\v4.0.30319',
           r'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\WPF',
           r'lib\SharpDX']

BINDIR = 'bin'
SRCDIR = 'src'

CSC = r'C:\Program Files (x86)\MSBuild\14.0\Bin\csc.exe'

#---------------------------------------
# FUNCTIONS
#---------------------------------------

@target
@depends_on('compile', 'content', 'libs')
def all():
    pass

@target
def clean():
    if os.path.isdir(BINDIR):
        remove_dir(BINDIR)

@target
def content():
    copy('res', os.path.join(BINDIR, 'Content'))

@target
def libs():
    copy('lib/SharpDX', BINDIR, '*.dll')

@target
def compile():
    create_dir(BINDIR)

    libdirs = ['/lib:' + ','.join(LIBDIRS)]
    libs    = ['/r:' + lib for lib in LIBS]
    out     = ['/out:' + os.path.join(BINDIR, NAME)]
    sources = ['/recurse:' + os.path.join(SRCDIR, '*.cs')]

    run_program(CSC, FLAGS + libdirs + libs + out + sources)

@target
def run():
    os.chdir(BINDIR)
    run_program(NAME)

#---------------------------------------
# SCRIPT
#---------------------------------------

pymake()
