#---------------------------------------
# IMPORTS
#---------------------------------------

import fnmatch
import os
import shutil
import subprocess
import sys

#---------------------------------------
# CONSTANTS
#---------------------------------------

CSC = 'C:\\Program Files (x86)\\MSBuild\\14.0\\Bin\\csc.exe'

FLAGS = [
#    '/debug',
#    '/define:DEBUG',
    '/define:RELEASE',
    '/langversion:6',
    '/nologo',
    '/optimize',
    '/platform:x64',
    '/target:exe'
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
# GLOBALS
#---------------------------------------

targets = {}

#---------------------------------------
# DECORATORS
#---------------------------------------

def target(*args):
    def wrapper(func):
        func.dependencies = args
        name = func.__name__
        targets[name] = func
        return func

    return wrapper

#---------------------------------------
# TARGETS
#---------------------------------------

@target('content', 'libs', 'program')
def all():
    pass

@target()
def clean():
    if os.path.isdir(BINDIR):
        shutil.rmtree(BINDIR)

@target()
def content():
    copy('Content', os.path.join(BINDIR, 'Content'))

@target()
def libs():
    copy('lib/SharpDX', BINDIR, '*.dll')

@target()
def program():
    if not os.path.exists(BINDIR):
        os.mkdir(BINDIR)

    flags   = FLAGS
    libdirs = ['/lib:' + ','.join(LIBDIRS)]
    libs    = ['/r:' + s for s in LIBS]
    out     = ['/out:' + os.path.join(BINDIR, TARGET)]
    sources = ['/recurse:' + os.path.join(SRCDIR, '*.cs')]

    subprocess.call([CSC] + flags + libdirs + libs + out + sources)

@target()
def run():
    subprocess.call([os.path.join(BINDIR, TARGET)])

#---------------------------------------
# FUNCTIONS
#---------------------------------------

def copy(srcpath, destpath, pattern=None):
    if os.path.isfile(srcpath):
        if pattern and not fnmatch.fnmatch(srcpath, pattern):
            return

        if os.path.exists(destpath):
            srcmt = os.path.getmtime(srcpath)
            destmt = os.path.getmtime(destpath)
            if srcmt <= destmt:
                return

        path, filename = os.path.split(destpath)
        print 'copying', filename, 'to', path

        shutil.copyfile(srcpath, destpath)
        return

    if not os.path.exists(destpath):
        os.makedirs(destpath)

    for s in os.listdir(srcpath):
        dest = os.path.join(destpath, s)
        src = os.path.join(srcpath, s)

        if os.path.isfile(s):
            copy(src, dest, pattern)
        else:
            copy(src, dest, pattern)

def make(target):
    func = targets[target]

    for dep in func.dependencies:
        make(dep)

    print
    func()

#---------------------------------------
# SCRIPT
#---------------------------------------

if __name__ == '__main__':
    s = 'all'
    if len(sys.argv) > 1:
        s = sys.argv[1]

    if s not in targets:
        print 'no such target:', s
        sys.exit()

    make(s)
