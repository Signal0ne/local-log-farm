FROM python:3.9

WORKDIR /app

COPY . /app/

RUN python -m venv venv && \
    /bin/bash -c "source venv/bin/activate" && \
    pip install --no-cache-dir -r requirements.txt

# No opentelemetry instrumentation
CMD ["python", "-u", "main.py"]

# With opentelemetry instrumentation
# CMD [ \
# "opentelemetry-instrument", \
# "--traces_exporter", "otlp", \
# "--metrics_exporter", "none", \
#  "python", "-u", "main.py" ]
