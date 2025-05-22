# Azure Service Bus Emulator Healthcheck

This is a simple healthcheck for the Azure Service Bus Emulator. 
It checks if the emulator is running and accessible based on the `Config.json` file.

It is available as the `jeremysv/azure-service-bus-emulator-healthcheck` Docker image.

### Usage

See [Service Bus Emulator](https://learn.microsoft.com/en-us/azure/service-bus-messaging/test-locally-with-service-bus-emulator?tabs=docker-linux-container) for details 
on using the Azure Service Bus Emulator.
The healthcheck will read the `Config.json` file (i.e. the config file provided to the emulator when it was started) 
and test the queues and topics defined in it.

Example `docker-compose.yml`:

```yaml
name: servicebus

services:
  sqledge:
    image: "mcr.microsoft.com/azure-sql-edge:latest"
    environment:
      ACCEPT_EULA: 'Y'
      MSSQL_SA_PASSWORD: 'StrongPassword1234!'
    healthcheck:
      test: '/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$${MSSQL_SA_PASSWORD}" -Q "SELECT 1" -b -o /dev/null'
      interval: 10s
      timeout: 3s
      retries: 5
      start_interval: 5s

  servicebus:
    image: jeremysv/azure-service-bus-emulator-healthcheck:latest
    volumes:
      - './Config.json:/ServiceBus_Emulator/ConfigFiles/Config.json:ro'
    ports:
      - '5672:5672/tcp'
      - '5300:5300/tcp'
    depends_on:
      sqledge:
        condition: service_healthy
    environment:
      SQL_SERVER: 'sqledge'
      ACCEPT_EULA: 'Y'
      SQL_WAIT_INTERVAL: '1'
      MSSQL_SA_PASSWORD: 'StrongPassword1234!'
    healthcheck:
      test: ['CMD', '/ServiceBusEmulator.Healthcheck']
      interval: 10s
      timeout: 3s
      retries: 5
      start_interval: 10s
```