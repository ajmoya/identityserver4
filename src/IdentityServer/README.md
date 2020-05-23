# 0. Construimos la imagen docker:
<i>docker build -t identityserver4 .</i>

# 1. Publicar la imagen como package en github
<i>docker login docker.pkg.github.com --username ajmoya</i>

### Obtener un access token personal desde settings developer en github con los scopes read:packages y write:packages
### Establecer el access token cuando solicite la password
<i>echo <accessToken> > accessToken.txt
  
cat accessToken.txt | docker login docker.pkg.github.com -u ajmoya --password-stdin</i>

### Ahora hay que crear un tag target_image que referencie a la imagen que se quiere subir a github
<i>docker tag IMAGE_ID docker.pkg.github.com/OWNER/REPOSITORY/IMAGE_NAME:VERSION</i>

### P.e, si la imagen identityserver4 tiene de ID ee769bfb6b11 y nuestro repo en github es labs-identityserver4 y queremos establecer la versión 0.2:
<i>docker tag ee769bfb6b11 docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0</i>

### Publish the image to GitHub Packages:
<i>docker push docker.pkg.github.com/OWNER/REPOSITORY/IMAGE_NAME:VERSION</i>

### Para nuestro caso:
<i>docker push docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0</i>

<br/><br/>

# 2. Para consumir la imagen docker ya publicada en github, primero debemos extraerla desde el registry de github:
<i>docker pull docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0</i>

### Y ya podemos ejecutarla:
<i>docker run -v C:/labs/identityserver/data:/app/data -v C:/labs/identityserver/logs:/app/logs -p 5010:5010 --name myIdentityServer4 docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0</i>

### Otra opción, si se quiere usar como imagen base en un fichero Dockerfile:
<i>FROM docker.pkg.github.com/ajmoya/labs-identityserver4/identityserver4:0.2.0</i>