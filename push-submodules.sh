#!/bin/bash
git submodule foreach 'git add . && git commit -m "$1" && git push'
