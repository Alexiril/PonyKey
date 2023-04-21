from os import listdir, getcwd, chdir
from os.path import isdir, join
from sys import argv
from tarfile import open as taropen

cwd = getcwd()
print(cwd)

if len(argv) < 2:
    raise AssertionError("Not enough arguments")

if not isdir(join(cwd, argv[1])):
    exit()

with taropen("assets.dat", "w:gz") as archive:
    chdir(join(cwd, argv[1]))
    for file in listdir(getcwd()):
        archive.add(file)

print(f"All files and folders from {join(cwd, argv[1])} were added to the archive.")
