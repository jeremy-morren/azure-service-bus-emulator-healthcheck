# Multi-arch "scratch" image containing only the healthcheck binaries in /Healthcheck.
#
# The build context must contain one directory per target architecture, each holding the
# published binaries for that architecture (the same files as the release .tar.gz assets):
#
#   <context>/amd64/...
#   <context>/arm64/...
#
# Built with `docker buildx build --platform linux/amd64,linux/arm64`. There are no RUN
# instructions, so no QEMU emulation is required to build the non-native platform.

FROM scratch

ARG TARGETARCH
COPY --chmod=555 ${TARGETARCH}/ /
