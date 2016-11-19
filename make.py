#---------------------------------------
# IMPORTS
#---------------------------------------

import os
import sys

sys.path.insert(0, os.path.join('build/pymake'))
from pymake import *

#---------------------------------------
# CONSTANTS
#---------------------------------------

CSC = 'C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\csc.exe'

FLAGS = [
#    '/debug',
#    '/define:DEBUG',
    '/nologo',
    '/o',
    '/platform:x64',
    '/target:winexe'
]

TARGET = 'Pong.exe'

LIBS = [
    'PresentationCore.dll',
    'System.IO.dll',
    'System.Runtime.dll',
    'WindowsBase.dll',

    'SharpDX.D3DCompiler.dll',
    'SharpDX.DXGI.dll',
    'SharpDX.Direct3D11.dll',
    'SharpDX.Mathematics.dll',
    'SharpDX.XAudio2.dll',
    'SharpDX.dll'
]

BINDIR = 'bin'
SRCDIR = 'src'

LIBDIRS = [
    'C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\',
    'C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\WPF',
    'lib\\SharpDX'
]

#---------------------------------------
# FUNCTIONS
#---------------------------------------

@target('compile', 'content', 'libs')
def all():
    pass

@target()
def clean():
    if os.path.isdir(BINDIR):
        remove_dir(BINDIR)

@target()
def content():
    copy('res', os.path.join(BINDIR, 'Content'))

@target()
def libs():
    copy('lib/SharpDX', BINDIR, '*.dll')

@target()
def compile():
    if not os.path.exists(BINDIR):
        os.mkdir(BINDIR)

    flags   = FLAGS
    libdirs = ['/lib:' + ','.join(LIBDIRS)]
    libs    = ['/r:' + s for s in LIBS]
    out     = ['/out:' + os.path.join(BINDIR, TARGET)]
    sources = ['/recurse:' + os.path.join(SRCDIR, '*.cs')]

    run_cmd(CSC, flags + libdirs + libs + out + sources)

@target()
def run():
    os.chdir(BINDIR)
    subprocess.call([TARGET])

#---------------------------------------
# SCRIPT
#---------------------------------------

make()
