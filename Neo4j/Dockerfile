FROM neo4j:latest
COPY ./csv/*.csv ./import/
COPY ./plugins/*.jar ./plugins/
RUN echo "dbms.unmanaged_extension_classes=me.neo4j.server.extension=/examples" >> ./conf/neo4j.conf