# run with docker, from Git project root
# sudo docker build . -t 'stroop:mono'
FROM	mono:6.8.0.96 AS builder

WORKDIR	/app

COPY    STROOP.sln .
COPY    STROOP/STROOP_linux.csproj STROOP/STROOP.csproj
COPY    STROOPUnitTests/STROOPUnitTests_linux.csproj STROOPUnitTests/STROOPUnitTests.csproj

RUN     nuget restore

COPY    . .

RUN     mv STROOP/STROOP_linux.csproj STROOP/STROOP.csproj \
 &&     mv STROOPUnitTests/STROOPUnitTests_linux.csproj STROOPUnitTests/STROOPUnitTests.csproj \
 &&     rm STROOP/packages.config \
 &&     rm STROOPUnitTests/packages.config \
 &&     rm -r /usr/lib/mono/gac/ICSharpCode.SharpZipLib # https://xamarin.github.io/bugzilla-archives/59/59304/bug.html

RUN     msbuild -property:Configuration=Release

FROM    mono:6.8.0.96

COPY    --from=builder /app/STROOP/bin/Release /STROOP