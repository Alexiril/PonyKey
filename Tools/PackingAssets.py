from os import listdir, getcwd, chdir
from os.path import isdir, join
from sys import argv
from tarfile import open as taropen

cwd = getcwd()

if len(argv) < 2:
    print(f"""Sorry. This is not how to use that script.
You need to start it giving it a name of the assets folder needed to pack and in the build folder,
that was previously assembled by monogame environment. Arguments: {argv}""")
    exit(-1)

if not isdir(join(cwd, argv[1])):
    exit()

with taropen("assets.dat", "w:gz") as archive:
    chdir(join(cwd, argv[1]))
    for file in listdir(getcwd()):
        archive.add(file)

print(f"All files and folders from {join(cwd, argv[1])} were added to the archive.")
