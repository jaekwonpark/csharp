FROM rockylinux:9

# install dotnet sdk 8.
RUN dnf update -y
RUN dnf install dotnet-sdk-8.0 -y

# copy .bash_profile
COPY .bash_profile /root/.bash_profile

# copy example C# application
ENV WORKSPACE /root/workspace
RUN mkdir -p ${WORKSPACE}/MyApp
COPY MyApp ${WORKSPACE}/MyApp

# Register the Microsoft RedHat repository
RUN curl https://packages.microsoft.com/config/rhel/7/prod.repo | tee /etc/yum.repos.d/microsoft.repo

# Install PowerShell
RUN yum install -y powershell

CMD ["/bin/bash", "-l"]

# To create a docker image, run 
# docker build -t powershell-linux .
# To run 
# docker run -it powershell-linux
