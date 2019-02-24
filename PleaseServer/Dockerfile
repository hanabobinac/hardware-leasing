FROM python:3

ADD src/* /

RUN pip install flask

ENV FLASK_APP server.py

CMD [ "flask", "run", "--host=0.0.0.0" ]
