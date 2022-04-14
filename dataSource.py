import os
import time

import pandas as pd
from bs4 import BeautifulSoup
from selenium import webdriver


def dataScraper(name):
    return pd.read_csv(f"https://stooq.pl/q/d/l/?s={name}&i=d")


def namesScraper():
    os.environ['MOZ_HEADLESS'] = '1'
    driver = webdriver.Firefox(executable_path='./webDrivers/geckodriver')
    driver.implicitly_wait(3)
    pageNotEmpty = True
    data = {}
    data_name = []
    data_symbol = []
    data_link = []
    i = 1
    while pageNotEmpty:
        print(f"reading page nr {i}")
        URL = f"https://stooq.pl/t/?i=513&v=0&l={i}"
        driver.get(URL)

        # time.sleep(3)
        if i == 1:
            driver.find_element_by_css_selector('.fc-button.fc-cta-consent.fc-primary-button').click()
            time.sleep(3)
            driver.refresh()
            # time.sleep(3)

        html = driver.page_source
        soup = BeautifulSoup(html, "html.parser")
        table = soup.find("table", attrs={"id": "fth1"})
        table_data = table.tbody.find_all("tr")

        for tr in table_data:
            name = tr.find(attrs={"id": "f10"})
            data_name.append(name.text)

            symbol = tr.find(attrs={"id": "f13", "width": "1%"})
            data_symbol.append(symbol.text)

            link = symbol.find("a")
            data_link.append("https://stooq.pl/" + link['href'])

        if not table_data:
            pageNotEmpty = False
        else:
            i += 1

    driver.close()
    data["Symbol"] = data_symbol
    data["Name"] = data_name
    data["Link"] = data_link
    df = pd.DataFrame(data)
    return df


# print(namesScraper())
# print(dataScraper('11B'))
