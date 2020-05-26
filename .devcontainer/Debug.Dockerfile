#-------------------------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See https://go.microsoft.com/fwlink/?linkid=2090316 for license information.
#-------------------------------------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS base

# Install the .NET Core SDK
RUN wget https://packages.microsoft.com/config/ubuntu/19.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb

# Avoid warnings by switching to noninteractive
ENV DEBIAN_FRONTEND=noninteractive

# This Dockerfile adds a non-root user with sudo access. Use the "remoteUser"
# property in devcontainer.json to use it. On Linux, the container user's GID/UIDs
# will be updated to match your local UID/GID (when using the dockerFile property).
# See https://aka.ms/vscode-remote/containers/non-root-user for details.
ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Configure apt and install packages
RUN apt-get update \
    && apt-get -y install --no-install-recommends apt-utils dialog 2>&1 \
    #
    # Verify git and needed tools are installed
    && apt-get -y install \
        git \
        openssh-client \
        less \
        unzip \
        iproute2 \
        procps \
        curl \
        apt-transport-https \
        gnupg2 \
        lsb-release \
    #
    # Create a non-root user to use if preferred - see https://aka.ms/vscode-remote/containers/non-root-user.
    && groupadd --gid $USER_GID $USERNAME \
    && useradd -s /bin/bash --uid $USER_UID --gid $USER_GID -m $USERNAME \
    # [Optional] Add sudo support for the non-root user
    && apt-get install -y sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME\
    && chmod 0440 /etc/sudoers.d/$USERNAME
    #
    # Install Azure Functions Core Tools v3
    #&& wget -qO- https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
    #&& mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
    #&& wget -q https://packages.microsoft.com/config/debian/9/prod.list \
    #&& mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
    #&& chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
    #&& chown root:root /etc/apt/sources.list.d/microsoft-prod.list \
    #&& apt-get update \
    #&& apt-get -y install azure-functions-core-tools-3 \
    #
    # Clean up
    #&& apt-get autoremove -y \
    #&& apt-get clean -y \
    #&& rm -rf /var/lib/apt/lists/*

# Azure CLI
#RUN apt-get update \
#    && sudo apt-get install ca-certificates \
#     curl apt-transport-https lsb-release gnupg
#
#RUN curl -sL https://packages.microsoft.com/keys/microsoft.asc \
#    && gpg --dearmor \
#    && sudo tee /etc/apt/trusted.gpg.d/microsoft.asc.gpg > /dev/null
#
#RUN AZ_REPO=$(lsb_release -cs) \
#    && echo "deb [arch=amd64] https://packages.microsoft.com/repos/azure-cli/ $AZ_REPO main" \
#    && tee /etc/apt/sources.list.d/azure-cli.list  
#
#RUN sudo apt-get update \
#    && sudo apt-get install azure-cli
    
# Uncomment to opt out of Func CLI telemetry gathering
#ENV FUNCTIONS_CORE_TOOLS_TELEMETRY_OPTOUT=true

# Switch back to dialog for any ad-hoc use of apt-get
ENV DEBIAN_FRONTEND=dialog

#RUN apt-get install dotnet-runtime-3.1
#RUN apt-get install dotnet-sdk-3.1