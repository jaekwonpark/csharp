# .bash_profile

# Get the aliases and functions
if [ -f ~/.bashrc ]; then
	. ~/.bashrc
fi

# User specific environment and startup programs

DEFAULT="\[\033[0m\]"
LIGHT_GREEN="\[\033[1;32m\]"
BLACK="\[\033[1;30m\]"

function parse_git_branch {
    git branch --no-color 2> /dev/null | sed -e '/^[^*]/d' -e 's/* \(.*\)/(\1)/'
}

PS1="[\u@${LIGHT_GREEN}rLinux:Net8${BLACK}:\w]${LIGHT_GREEN}\$(parse_git_branch)
$DEFAULT\$"


PATH=$PATH:$HOME/bin:/usr/local/dotnet

alias l='ls -alF'
alias h='history'
