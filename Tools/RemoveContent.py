from os import getcwd
from os.path import join, exists
from shutil import rmtree

if (exists(join(getcwd(), "Content"))):
    rmtree(join(getcwd(), "Content"))
else:
    print("""Sorry. This is not how to use that script.
You need to start it in the build folder that contains Content folder,
that was previously assembled by monogame environment.""")
