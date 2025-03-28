# Sử dụng image SQL Server chính thức
FROM mcr.microsoft.com/mssql/server:2019-latest

# Thiết lập môi trường
ENV SA_PASSWORD="Adminxyz22#"
ENV ACCEPT_EULA="Y"

# Copy script SQL vào container
COPY init.sql /usr/src/app/init.sql

# Mở cổng SQL Server
EXPOSE 1433

# Chạy SQL Server và thực thi script init.sql sau khi khởi động
# CMD /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P Adminxyz22# -i /usr/src/app/init.sql -C && \
#     wait