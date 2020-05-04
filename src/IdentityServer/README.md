# Construimos la imagen docker:
docker build -t identityserver4 .

# Publicar la imagen como package en github
docker login docker.pkg.github.com --username ajmoya

# Obtener un access token personal desde settings developer en github con los siguientes scopes:
<i>read:packages y write:packages</i>


# Establecer el access token cuando solicite la password
echo <accessToken> > accessToken.txt
cat accessToken.txt | docker login docker.pkg.github.com -u ajmoya --password-stdin

# Ahora hay que crear un tag target_image que referencie a la imagen que se quiere subir a github
docker tag IMAGE_ID docker.pkg.github.com/OWNER/REPOSITORY/IMAGE_NAME:VERSION

# P.e, si la imagen identityserver4 tiene de ID ee769bfb6b11 y nuestro repo en github es labs-identityserver4 y queremos establecer la versión 0.2:
docker tag ee769bfb6b11 docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0

# Publish the image to GitHub Packages:
docker push docker.pkg.github.com/OWNER/REPOSITORY/IMAGE_NAME:VERSION

# Para nuestro caso:
docker push docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0


# Para consumir la imagen docker ya publicada en github, primero debemos extraerla desde el registry de github:
docker pull docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0

# Y ya podemos ejecutarla:
docker run -v C:/dataDocker:/data -p 5010:5010 --name myIdentityServer docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0

# Otra opción, si se quiere usar como imagen base en un fichero Dockerfile:
FROM docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0
