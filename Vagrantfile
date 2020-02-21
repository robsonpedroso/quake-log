Vagrant.configure("2") do |config|

  config.vm.define "ubuntu" do |ubuntu|
    # configuration ubuntu image with ubuntu 19
    ubuntu.vm.box = "ubuntu/eoan64"

    # configuration timeout for 20 minutes
    ubuntu.vm.boot_timeout = 1200
    
    # configuration IP for visualization in host
    ubuntu.vm.network "private_network", ip: "192.168.100.10"

    # configuration folders
    ubuntu.vm.synced_folder "./configs", "/configs"
    ubuntu.vm.synced_folder ".", "/quake"
    ubuntu.vm.synced_folder ".", "/vagrant", disabled: true

    ubuntu.vm.provision "shell", inline: <<-SHELL
      apt-get update
      apt-get install -y docker.io
      cat /configs/id_rsa.pub >> .ssh/authorized_keys
      docker build -f Dockerfile -t robsonpedroso/quake .
      docker run -d -p 80:8080 robsonpedroso/quake
    SHELL
  end
end
