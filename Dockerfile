# run with docker, from Git project root
# sudo docker build . -t 'stroop:mono'
FROM	mono:6.8.0.96 AS builder

WORKDIR	/app

COPY    STROOP.sln .
COPY    STROOP/STROOP_linux.csproj STROOP/STROOP.csproj
COPY    STROOPUnitTests/STROOPUnitTests_linux.csproj STROOPUnitTests/STROOPUnitTests.csproj

RUN     nuget restore

COPY    STROOP/ STROOP/
COPY    STROOPUnitTests/ STROOPUnitTests/

RUN     mv STROOP/STROOP_linux.csproj STROOP/STROOP.csproj \
 &&     mv STROOPUnitTests/STROOPUnitTests_linux.csproj STROOPUnitTests/STROOPUnitTests.csproj \
 &&     rm STROOP/packages.config \
 &&     rm STROOPUnitTests/packages.config \
 &&     rm -r /usr/lib/mono/gac/ICSharpCode.SharpZipLib # https://xamarin.github.io/bugzilla-archives/59/59304/bug.html

RUN     msbuild -property:Configuration=Release

FROM    mono:6.8.0.96

RUN     dpkg --add-architecture i386

RUN     apt-get update \
 &&     apt-get install -y \
            libc6-dbg libc6-dbg:i386 lib32stdc++6 \
            zlib1g:i386 \
            libgtk2.0-0:i386 \
            libsdl2-2.0-0:i386 \
            libpangox-1.0-0:i386 libpangoxft-1.0-0:i386 \
            libsdl1.2debian:i386 \
            libgl1:i386 \
            libglu1-mesa:i386 \
            libstdc++5:i386 \
            mesa-utils:i386 \
            libgtk2.0-dev:i386

COPY    mupen/ /mupen/
COPY    --from=builder /app/STROOP/bin/Release /STROOP