## Konfiguration für nginx

```
server {
    listen   80;
    server_name example.com;

    location /l {
        proxy_pass localhost:9000;
        proxy_redirect     off;
        proxy_set_header   Host             $host;
        proxy_set_header   X-Real-IP        $remote_addr;
        proxy_set_header   X-Forwarded-For  $proxy_add_x_forwarded_for;
    }
}
```

## docker run-Befehl:

```
-e URL_PREFIX="https://example.com/"
-e ADMIN_PASSWORD="my-secret-password"
```
