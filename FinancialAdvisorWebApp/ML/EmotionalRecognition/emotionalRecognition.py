from tensorflow.keras.preprocessing.image import img_to_array
import imutils
import cv2
from keras.models import load_model
from keras.models import model_from_json
import numpy as np
import json
import os
import pathlib
from twilio.rest import Client
from pydub import AudioSegment
import requests
import pyaudio
import pandas as pd
import IPython.display as ipd 
import sys
import librosa

pathLocal = (str((pathlib.Path().absolute())).replace("\\", "/"))+"/ML/EmotionalRecognition"
room_sid = sys.argv[1]
account_sid = os.environ['TWILIO_ACCOUNT_SID']
auth_token = os.environ['TWILIO_AUTH_TOKEN']
client = Client(account_sid, auth_token)
# parameters for loading data and images
detection_model_path = pathLocal+'/haarcascade_files/haarcascade_frontalface_default.xml'
emotion_model_path = pathLocal+'/models/_mini_XCEPTION.102-0.66.hdf5'

CHUNK = 1024
FORMAT = pyaudio.paInt16
CHANNELS = 2
RATE = 44100
emotions = ["Anger", "disgust", "fear", "happy", "Neutral", "sad", "surprise"]
EMOTIONS = ["angry", "disgust", "scared","happy", "sad", "surprised", "neutral"]

face_detection = cv2.CascadeClassifier(detection_model_path)
emotion_classifier = load_model(emotion_model_path, compile=False)

            
recording_sid = ""
recordingsByRoom = client.video.recordings.list(
    grouping_sid=[room_sid], limit=20)
for record in recordingsByRoom:
    if record.type == "audio":
        recording_sid = record.sid
        uri = "https://video.twilio.com/v1/Recordings/{}/Media".format(recording_sid)
        response = client.request("GET", uri)
        media_location = json.loads(response.text).get("redirect_to")
        r = requests.get(media_location, allow_redirects=True)
        open(pathLocal+'/record.mka', 'wb').write(r.content)
        AudioSegment.from_file(
            pathLocal+'/record.mka').export(pathLocal+"/record.wav", format="wav")
    elif record.type == "video":
        recording_sid = record.sid
        uri = "https://video.twilio.com/v1/Recordings/{}/Media".format(recording_sid)
        response = client.request("GET", uri)
        media_location = json.loads(response.text).get("redirect_to")
        r = requests.get(media_location, allow_redirects=True)
        open(pathLocal+'/record.mkv', 'wb').write(r.content)


# ********************************************** SPEECH EMOTIONAL RECOGNITION **********************************************
def get_audio_features(audio_path,sampling_rate):
    X, sample_rate = librosa.load(audio_path ,res_type='kaiser_fast',duration=2.5,sr=sampling_rate*2,offset=0.5)
    sample_rate = np.array(sample_rate)

    y_harmonic, y_percussive = librosa.effects.hpss(X)
    pitches, magnitudes = librosa.core.pitch.piptrack(y=X, sr=sample_rate)

    mfccs = np.mean(librosa.feature.mfcc(y=X,sr=sample_rate,n_mfcc=13),axis=1)

    pitches = np.trim_zeros(np.mean(pitches,axis=1))[:20]

    magnitudes = np.trim_zeros(np.mean(magnitudes,axis=1))[:20]

    C = np.mean(librosa.feature.chroma_cqt(y=y_harmonic, sr=sampling_rate),axis=1)
    
    return [mfccs, pitches, magnitudes, C]

json_file = open(pathLocal+'/utils/model.json', 'r')
loaded_model_json = json_file.read()
json_file.close()
loaded_model = model_from_json(loaded_model_json)
loaded_model.load_weights(
    pathLocal+"/Trained_Models/Speech_Emotion_Recognition_Model.h5")
ipd.Audio(pathLocal+"/record.wav")
demo_mfcc, demo_pitch, demo_mag, demo_chrom = get_audio_features(
    pathLocal+"/record.wav", 20000)
mfcc = pd.Series(demo_mfcc)
pit = pd.Series(demo_pitch)
mag = pd.Series(demo_mag)
C = pd.Series(demo_chrom)
demo_audio_features = pd.concat([mfcc, pit, mag, C], ignore_index=True)
demo_audio_features = np.expand_dims(demo_audio_features, axis=0)
demo_audio_features = np.expand_dims(demo_audio_features, axis=2)
livepreds = loaded_model.predict(demo_audio_features,
                                 batch_size=32,
                                 verbose=1)
indexSpeechEmotion = livepreds.argmax(axis=1).item()

# ********************************************** FACE EMOTIONAL RECOGNITION **********************************************

cv2.namedWindow('your_face')
# cap = cv2.VideoCapture(0)
cap = cv2.VideoCapture(pathLocal+'/record.mkv')  # Video file source
while cap.isOpened():
    ret, frame = cap.read()
    if ret:
        # reading the frame
        frame = imutils.resize(frame, width=300)
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        faces = face_detection.detectMultiScale(gray, scaleFactor=1.1, minNeighbors=5, minSize=(30, 30), flags=cv2.CASCADE_SCALE_IMAGE)

        canvas = np.zeros((250, 300, 3), dtype="uint8")
        frameClone = frame.copy()
        if len(faces) > 0:
            faces = sorted(faces, reverse=True, key=lambda x: (x[2] - x[0]) * (x[3] - x[1]))[0]
            (fX, fY, fW, fH) = faces
            roi = gray[fY:fY + fH, fX:fX + fW]
            roi = cv2.resize(roi, (64, 64))
            roi = roi.astype("float") / 255.0
            roi = img_to_array(roi)
            roi = np.expand_dims(roi, axis=0)

            preds = emotion_classifier.predict(roi)[0]
            emotion_probability = np.max(preds)
            label = EMOTIONS[preds.argmax()]
        else:
            continue

        angry = 0
        disgust = 0
        scared = 0
        happy = 0
        sad = 0
        surprised = 0
        neutral = 0
        total = 0
        for (i, (emotion, prob)) in enumerate(zip(EMOTIONS, preds)):
            text = "{}: {:.2f}%".format(emotion, prob * 100)
            if emotion == "angry":
                angry = angry+(prob * 100)
                total = total + 1
            elif emotion == "disgust":
                disgust = disgust+(prob * 100)
            elif emotion == "scared":
                scared = scared+(prob * 100)
            elif emotion == "happy":
                happy = happy+(prob * 100)
            elif emotion == "sad":
                sad = sad+(prob * 100)
            elif emotion == "surprised":
                surprised = surprised+(prob * 100)
            elif emotion == "neutral":
                neutral = neutral+(prob * 100)

            w = int(prob * 300)
            cv2.rectangle(canvas, (7, (i * 35) + 5),
                          (w, (i * 35) + 35), (0, 0, 255), -1)
            cv2.putText(canvas, text, (10, (i * 35) + 23),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.45, (255, 255, 255), 2)
            cv2.putText(frameClone, label, (fX, fY - 10),
                        cv2.FONT_HERSHEY_SIMPLEX, 0.45, (0, 0, 255), 2)
            cv2.rectangle(frameClone, (fX, fY),
                          (fX + fW, fY + fH), (0, 0, 255), 2)

        cv2.imshow('your_face', frameClone)
        cv2.imshow("Probabilities", canvas) 
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break
    else:
        break


deviation = np.nanstd([angry, disgust, scared, happy, sad, surprised, neutral])
data = {}
data['average'] = {'angry': (angry/total), 'disgust': (disgust/total), 'scared': (scared/total), 'happy': (happy/total),
                   'sad': (sad/total), 'surprised': (surprised/total), 'neutral': (neutral/total)}
data['deviation'] = deviation

data['speechRecognition'] = indexSpeechEmotion
json_data = json.dumps(data)


print(json_data)
cap.release()
os.remove(pathLocal+'/record.wav')
os.remove(pathLocal+'/record.mka')
os.remove(pathLocal+'/record.mkv')
cv2.destroyAllWindows()