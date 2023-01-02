FROM rockylinux:9
# install necessary pacakges
RUN yum install -y wget

# install dotnet sdk 6. Latest version as of now is 6.0.404
ENV DOTNET_INSTALL_FILE dotnet-sdk-6.0.404-linux-x64.tar.gz
RUN wget https://download.visualstudio.microsoft.com/download/pr/40bc5873-1a39-4a47-9df5-b481869594a6/8455eab2092cc1321ac055f016ef90cf/${DOTNET_INSTALL_FILE} -P /tmp
RUN mkdir /usr/local/dotnet
RUN tar xvfz /tmp/${DOTNET_INSTALL_FILE} -C /usr/local/dotnet
RUN rm /tmp/${DOTNET_INSTALL_FILE}

# copy .bash_profile
COPY .bash_profile /root/.bash_profile

#Register the Microsoft RedHat repository
RUN curl https://packages.microsoft.com/config/rhel/7/prod.repo | tee /etc/yum.repos.d/microsoft.repo

# Install PowerShell
RUN yum install -y powershell

CMD ["/bin/bash", "-l"]

# To create a docker image, run 
# docker build -t powershell-linux .
# To run 
# docker run -it poerwhell-linux
