import json
from pathlib import Path
from types import SimpleNamespace

import pandas as pd
import pika

import dataContainer
from company import Company
from dataSource import namesScraper
from profile import Profile


def get_companies():
    out = []
    for i, company_row in dataContainer.companies.iterrows():
        out.append(company_row_to_class(company_row))
    return out


def get_profiles():
    out = []
    for i, profile_row in dataContainer.profiles.iterrows():
        out.append(profile_row_to_class(profile_row))
    return out


def initializeData():
    initializeCompanies()
    initializeProfiles()


def initializeCompanies():
    dataContainer.companies = namesScraper()


def initializeProfiles():
    path_to_file = 'profiles_data.csv'
    path = Path(path_to_file)

    if path.is_file():
        dataContainer.profiles = pd.read_csv(path, index_col=0)
    else:
        data = {"Name": [], "Img": [], "TrackedCompanies": []}
        dataContainer.profiles = pd.DataFrame(data)
        saveProfilesData()


def saveProfilesData():
    path_to_file = 'profiles_data.csv'
    path = Path(path_to_file)
    to_save = dataContainer.profiles

    to_save.to_csv(path)


def constructCompany(data):
    row = data.iloc[0]
    out = Company(row['Name'], row['Code'], row['Link'], None)
    return out


def constructProfile(data):
    row = data.iloc[0]
    out = Profile(row['Name'], row['Img'], row['TrackedCompanies'])
    return out


# TODO DLA KSIĘDZA
def get_image(company_name):
    return [[1, 2, 3, 4, 5, 6]]


def handler(message, ch, properties):
    code = message[:3]
    rest = message[4:]

    if code == "GET":
        get_handler(rest, ch, properties)
    elif code == "SND":
        send_handler(rest)


def get_handler(message, ch, properties):
    code = message[:3]
    rest = message[4:]

    if code == "PRS":
        profiles_sender(ch, properties)
    elif code == "CMP":
        companies_sender(ch, properties)
    elif code == "IMG":
        image_sender(rest, ch, properties)


def profiles_sender(ch, properties):
    code = "SND:PRS:"
    data = json.dumps(get_profiles(), default=vars)
    ch.basic_publish('', routing_key=properties.reply_to, body=code + data)


def companies_sender(ch, properties):
    code = "SND:CMP:"
    data = json.dumps(get_companies(), default=vars)
    ch.basic_publish('', routing_key=properties.reply_to, body=code + data)


def image_sender(rest, ch, properties):
    code = "SND:IMG:"
    company_name = rest
    company_found = dataContainer.companies.loc[dataContainer.companies['Name'] == company_name]
    if len(company_found) > 0:
        company_img = get_image(company_name)
        ch.basic_publish('', routing_key=properties.reply_to, body=code + company_name + ":" + json.dumps(company_img))


def send_handler(message):
    code = message[:3]
    rest = message[4:]

    if code == "NUS":
        new_user_created(rest)
    elif code == "DUS":
        user_deleted(rest)
    elif code == "NCM":
        tracked_company_added(rest)
    elif code == "DCM":
        tracked_company_removed(rest)


def new_user_created(raw):
    profile_data = json.loads(raw, object_hook=lambda d: SimpleNamespace(**d))
    dataContainer.profiles = dataContainer.profiles.append(
        {"Name": profile_data.Name, "Img": profile_data.Img, "TrackedCompanies": profile_data.TrackedCompanies},
        ignore_index=True)


def user_deleted(raw):
    profile_name = raw
    dataContainer.profiles = dataContainer.profiles.loc[dataContainer.profiles["Name"] != profile_name]


def tracked_company_added(raw):
    profile_name = raw[:raw.find(':')]
    company_name = raw[raw.find(':') + 1:]

    possible_profile = dataContainer.profiles.loc[dataContainer.profiles["Name"] == profile_name]
    possible_company = dataContainer.companies.loc[dataContainer.companies['Name'] == company_name]

    if len(possible_profile) > 0 and len(possible_company) > 0:
        possible_tracked_company = [x for x in possible_profile.iloc[0]['TrackedCompanies'] if x == company_name]

        if len(possible_tracked_company) == 0:
            profile_index = dataContainer.profiles.index[dataContainer.profiles["Name"] == profile_name][0]
            tracked_companies = dataContainer.profiles.loc[profile_index, 'TrackedCompanies']
            tracked_companies.append(company_name)
            dataContainer.profiles.loc[profile_index, 'TrackedCompanies'] = tracked_companies


def tracked_company_removed(raw):
    profile_name = raw[:raw.find(':')]
    company_name = raw[raw.find(':') + 1:]

    possible_profile = dataContainer.profiles.loc[dataContainer.profiles["Name"] == profile_name]
    possible_company = dataContainer.companies.loc[dataContainer.companies['Name'] == company_name]

    if len(possible_profile) > 0 and len(possible_company) > 0:
        possible_tracked_company = [x for x in possible_profile.iloc[0]['TrackedCompanies'] if x == company_name]

        if len(possible_tracked_company) > 0:
            profile_index = dataContainer.profiles.index[dataContainer.profiles["Name"] == profile_name][0]
            tracked_companies = dataContainer.profiles.loc[profile_index, 'TrackedCompanies']
            tracked_companies.remove(company_name)
            dataContainer.profiles.loc[profile_index, 'TrackedCompanies'] = tracked_companies


def profile_class_to_row(profile):
    return {"Name": profile.Name, "Img": profile.Img, "TrackedCompanies": profile.TrackedCompanies}


def company_class_to_row(company):
    return {"Name": company.Name, "Code": company.Code, "Link": company.Link, "Img": company.Img}


def profile_row_to_class(profile):
    return Profile(profile.Name, profile.Img, profile.TrackedCompanies)


def company_row_to_class(company):
    return Company(company.Name, company.Code, company.Link, company.Img)


def on_request_message_received(ch, method, properties, body: bytes):
    print(f"Received Request")
    print(body.decode())
    handler(body.decode(), ch, properties)
    print(dataContainer.companies)
    print(dataContainer.profiles)


def server():
    # C1 = Company("Amazon1", "AMZ1", "https://stooq.pl/q/?s=11b", None)
    # C2 = Company("Amazon2", "AMZ2", "https://stooq.pl/q/?s=ale", None)
    # dataContainer.scraped_Companies = dataContainer.scraped_Companies.append(company_class_to_row(C1),
    #                                                                         ignore_index=True)
    # dataContainer.scraped_Companies = dataContainer.scraped_Companies.append(company_class_to_row(C2),
    #                                                                          ignore_index=True)
    #
    # P1 = Profile("Adam", "dupny link", [])
    # P2 = Profile("Adam2", "dupny link2", ["Amazon1", "Amazon2"])
    # dataContainer.profiles_df = dataContainer.profiles_df.append(profile_class_to_row(P1), ignore_index=True)
    # dataContainer.profiles_df = dataContainer.profiles_df.append(profile_class_to_row(P2), ignore_index=True)

    initializeData()

    connection_parameters = pika.ConnectionParameters('localhost')

    connection = pika.BlockingConnection(connection_parameters)

    channel = connection.channel()

    channel.queue_declare(queue='request-queue', auto_delete=True)

    channel.basic_consume(queue='request-queue', auto_ack=True,
                          on_message_callback=on_request_message_received)
    # channel
    print("Starting Server")

    channel.start_consuming()
