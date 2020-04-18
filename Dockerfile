# run with docker, from Git project root
# docker build . -t 'stroop'
FROM	mono:6.8.0.96

RUN     apt-get update \
 &&     apt-get install -y \
            libopentk1.1-cil \
            libgtk2.0-0 \
            libgl1-mesa-dev \
            libglu1-mesa-dev \
            freeglut3-dev \
            mesa-utils

WORKDIR	/app

COPY    STROOP.sln .
COPY    STROOP/STROOP_linux.csproj STROOP/STROOP.csproj
COPY    STROOP/packages.config STROOP/
COPY    STROOPUnitTests/STROOPUnitTests.csproj STROOPUnitTests/packages.config STROOPUnitTests/

RUN     /usr/bin/nuget restore

COPY	. .

RUN     mv STROOP/STROOP_linux.csproj STROOP/STROOP.csproj

RUN     msbuild
