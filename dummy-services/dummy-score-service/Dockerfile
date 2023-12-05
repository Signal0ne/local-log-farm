FROM golang:1.20.3-bullseye as base

WORKDIR /app
COPY ./ .
RUN go mod download


FROM base as build

WORKDIR /app
RUN go build -o /app/bin/scoreservice ./cmd/...


FROM golang:1.20.3-bullseye as release

COPY --from=build /app/bin/ /app/bin/
COPY --from=base /app/.local.env /go
CMD ["/app/bin/scoreservice"]