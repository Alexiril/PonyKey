from gzip import open as gopen
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
framesAmount: int = int(input("Set frames amount: "))

resultBytes += len(mark).to_bytes(4, 'big')
resultBytes += mark.encode()

resultBytes += pack(">f", framerate)

resultBytes += framesAmount.to_bytes(4, 'big')

for i in range(1, framesAmount + 1):
    currentFrame: str = input(f"Set {i} frame address: ")
    while not isfile(join(getcwd(), currentFrame)):
        currentFrame: str = input(f"Not correct file. Set correct {i} frame address: ")
    with open(currentFrame) as file:
        currentFrame = file.read()
        resultBytes += len(currentFrame).to_bytes(4, 'big')
        resultBytes += currentFrame.encode()

with gopen(join(getcwd(), argv[1] + ".asvg"), 'wb') as file:
    file.write(resultBytes)

print(f"{framesAmount} frames were packed into a package")
