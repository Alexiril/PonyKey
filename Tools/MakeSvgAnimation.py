from bz2 import compress
from os import getcwd
from os.path import isfile, join
from struct import pack
from sys import argv

version: str = "1.0"

print(f"SVG animation packing asset script v{version}")

if len(argv) < 2:
    print(f"""Sorry. This is not how to use that script.
You need to get it the package file name,
that was previously assembled by monogame environment.""")
    exit(-1)

fileName: str = argv[1]
resultBytes: bytes = b''
mark: str = "PonyKey"
framerate: float = float(input("Set framerate: "))
filesRanged: bool = True if input("Are filenames ranged? (y/n) ")[0].lower() == 'y' else False
filesRange = (0, 0)
if filesRanged:
    filesRanged = list(map(int, input("Set filenames range (from, to): ").replace(" ", "").split(",")))
framesAmount: int = filesRanged[1] - filesRanged[0] + 1 if filesRanged else int(input("Set frames amount: "))


resultBytes += len(mark).to_bytes(4, 'big')
resultBytes += mark.encode()

resultBytes += pack(">f", framerate)

resultBytes += framesAmount.to_bytes(4, 'big')

rangeStart = filesRange[0] if filesRanged else 1
rangeEnd = filesRanged[1] + 1 if filesRanged else framesAmount

for i in range(rangeStart, rangeEnd):
    if not filesRanged:
        currentFrame: str = input(f"Set {i} frame address: ")
        while not isfile(join(getcwd(), currentFrame)):
            currentFrame: str = input(f"Not correct file. Set correct {i} frame address: ")
    else:
        currentFrame: str = str(i) + ".svg"
        if not isfile(join(getcwd(), currentFrame)):
            raise FileNotFoundError("Not found an animation range file.")
    with open(currentFrame) as file:
            currentFrame = file.read()
            resultBytes += len(currentFrame).to_bytes(4, 'big')
            resultBytes += currentFrame.encode()

initialLength = len(resultBytes)
resultBytes = compress(resultBytes)
print(f"Compression ratio: {initialLength / len(resultBytes)}")
with open(join(getcwd(), argv[1] + ".asvg"), 'wb') as file:
    file.write(resultBytes)

print(f"{framesAmount} frames were packed into a package")
