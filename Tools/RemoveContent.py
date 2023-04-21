from os import getcwd
from os.path import join, exists
from shutil import rmtree

if (exists(join(getcwd(), "Content"))):
    rmtree(join(getcwd(), "Content"))
