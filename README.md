# Proyecto dojo.net.bp
Este proyecto es un REST API desarrollado en .Net 6, siguiendo la Arquitectura Limpia y patron Repository. En la solucion, se encuentran 4 proyectos, que son:
  1. dojo.net.bp.api: Es el API como tal donde se encuentran los endpoints para las diferentes operaciones de CRUD de clientes, cuentas y movimientos, 
                      asi como tambien, el endpoint para el estado de cuenta.
  2. dojo.net.bp.application: Este proyecto contiene los servicios que mantienen la logica del negocio. Desde aqui el endpoint se comunica con el proveedor
                              de datos para la consulta/actualizacion de los mismos. Tambien contiene las interfaces activas y pasivas para la comunicacion
                              entre la capa del API y la capa de Infraestructura.
  3. dojo.net.bp.domain: Este proyecto contiene las entidades del negocio, asi como tambien, entidades DTO para la transferencia de datos entre el API y la
                          capa de servicios.
  4. dojo.net.bp.infrastructure: Este proyecto contiene las clases Repositorio para la persistencia de los datos en el motor de SQL Server.

Este API maneja transaccionabilidad a nivel de EF cuando se desea insertar Movimientos.
  
En la carpeta principal, se encuentra el archivo json de Postman con nombre "Dojo .Net BP.postman_collection" para las pruebas necesarias del API.



# Limitaciones del proyecto
El proyecto es totalmente funcional, sin embargo, no se logró insertar pruebas unitarias ni tampoco contenerizarlo en Docker.



# Instrucciones de Despliegue
*BD:*
En la raiz de este proyecto se encuentra una carpeta con nombre DbScripts. Dentro de ella se encuentra el script para la creacion de las tablas y sus relaciones. 
En desarrollo, la base fue nombrada como "dojo.net.bp". En caso de que se desee que la base de datos tenga otro nombre, tambien se debe cambiar dentro de este script en la 1era linea del USE. 

*API:*
Puesto que el API no esta contenerizado usando docker, se debe desplegar el API como servicio para su uso. Afortunadamente, el API ya cuenta con lo necesario
para el despliegue como servicio de Windows. Para desplegarlo, se debe publicar usando Visual Studio 2022, dando click derecho sobre el proyecto dojo.net.bp.api, seleccionar "Publish". Se lo debe desplegar como "Self-contained deployment mode" con las siguientes configuraciones: 
![image](https://user-images.githubusercontent.com/28907922/179029614-700c19c1-635a-45f8-9f8c-82255494954b.png)

Para crear el servicio de Windows, seguir los pasos de la siguiente url: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-6.0&tabs=visual-studio#create-and-manage-the-windows-service

Una vez creado e iniciado el servicio de Windows, el API esta listo para escuchar peticiones a traves del puerto 8081. En caso que se desee cambiar el puerto de escucha, se debe cambiar en el archivo de appsettings.json en la zona de Kestrel:
![image](https://user-images.githubusercontent.com/28907922/179031154-26d6e137-4879-4a54-a1b8-9ac2a6149a31.png)

Es preferible configurar el servicio para que se ejecute con las credenciales de la cuenta del sistema local, pero si se desea que se ejecute con otras credenciales, se debe cambiar dicha configuracion en los Servicios de Windows:
![image](https://user-images.githubusercontent.com/28907922/179032711-5fe2f209-d80a-423a-8025-bfbf17854668.png)
![image](https://user-images.githubusercontent.com/28907922/179032847-5a5b5fa0-99f9-42fe-81f2-d1d11a8f9752.png)


Asimismo, cambiar la cadena de conexion en el archivo appsettings.json dentro del proyecto dojo.net.bp.api, apuntando a la base de datos 
correspondiente con las credenciales necesarias.

Con los cambios hechos, se debe reiniciar el servicio de Windows segun el nombre que se le dio al momento de crearlo. Ya con estos cambios hechos, el API esta listo para ser probado!
